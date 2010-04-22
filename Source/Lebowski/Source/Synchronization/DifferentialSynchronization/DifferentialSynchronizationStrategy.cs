using System;
using System.Timers;
using System.Collections.Generic;
using Lebowski.TextModel;
using Lebowski.Net;
using DiffMatchPatch;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
	public enum State
	{
		WaitingForToken,
		HavingToken
	};
	
	[Serializable]
	class DiffMessage
	{
		public string Diff { get; protected set; }
		
		public DiffMessage(string diff)
		{
			Diff = diff;
		}
		
		public override string ToString()
		{
			return String.Format("DiffMessage({0})", Diff);
		}
	}
	
	public class DifferentialSynchronizationStrategy
	{
		public int SiteId { get; protected set; }
		IConnection Connection;
		ITextContext Context;
		StringTextContext Shadow;
		diff_match_patch DiffMatchPatch = new diff_match_patch();
		
		public bool HasChanged { get; private set; }
		public State State { get; protected set; }
		
		
		public bool EnableAutoFlush { get; set; }
		private Timer FlushTimer;
		
		public DifferentialSynchronizationStrategy(int siteId, ITextContext context, IConnection connection)
		{
			SiteId = siteId;
			DiffMatchPatch =  new diff_match_patch();
			Context = context;
			Connection = connection;
			Shadow = new StringTextContext();
			
			HasChanged = false;
			
			EnableAutoFlush = false;
			FlushTimer = new Timer(250);
			FlushTimer.AutoReset = false;
			FlushTimer.Enabled = EnableAutoFlush;
			FlushTimer.Elapsed += delegate {
				System.Console.WriteLine("{0}: FlushTimer", SiteId);
				lock(this)
				{
					FlushToken();
				}
			};
			
			State = SiteId == 0 ? State.HavingToken : State.WaitingForToken;
			
			Connection.Received += delegate(object sender, ReceivedEventArgs e)
			{
				System.Console.WriteLine("{0} received: {1}", SiteId, e.Message);
				lock(this)
				{
					if(e.Message is DiffMessage)
					{
						System.Diagnostics.Debug.Assert(State == State.WaitingForToken);
						State = State.HavingToken;
						DiffMessage diffMessage = (DiffMessage)e.Message;
						if(diffMessage.Diff != "") 
						{
							ApplyPatches(diffMessage.Diff);
						}
						
						if(HasChanged)
						{
							HasChanged = false;
							State = State.WaitingForToken;	
							SendPatches();
						}
						else if(EnableAutoFlush)
						{
							FlushTimer.Stop();
							FlushTimer.Start();
						}
					}
					else 
					{
						throw new Exception(String.Format("Encountered unknown message type '{0}'", e.GetType().Name));
					}
				}
			};
			
			Context.Changed += delegate(object sender, ChangeEventArgs e)
			{
				lock(this)
				{
					if(e.Issuer == this)
						return;
					if(State == State.HavingToken)
					{
						HasChanged = false;
						State = State.WaitingForToken;					
						SendPatches();
					}
					else
					{
						HasChanged = true;
					}
				}
			};
		}
		
		protected void SendPatches()
		{
			// Create patch local
			var localDiffs = DiffMatchPatch.diff_main(Shadow.Data, Context.Data);
			Shadow.Data = Context.Data;		
			var delta = DiffMatchPatch.diff_toDelta(localDiffs);
			Connection.Send(new DiffMessage(delta));
		}
		
		void ApplyPatches(string delta)
		{
			string old = Context.Data;
			
			// Apply diff to shadow
			var diffs = DiffMatchPatch.diff_fromDelta(Shadow.Data, delta);
			var shadowPatch = DiffMatchPatch.patch_make(Shadow.Data, diffs);
			Shadow.Data = (string)DiffMatchPatch.patch_apply(shadowPatch, Shadow.Data)[0];
			
			/* For the real context, we have to perform each operation by hand,
			so the context can do things like moving around the selection area */
			var textPatch = DiffMatchPatch.patch_make(Context.Data, diffs);
			foreach(Patch patch in textPatch)
			{
				System.Console.WriteLine(patch);
			}
			int[] offsets = {Context.SelectionStart, Context.SelectionEnd};
			Context.Data = (string)patch_apply(textPatch, Context.Data, offsets)[0];			
			Context.SetSelection(offsets[0], offsets[1]);
			
			Console.WriteLine("Was {0} now {1}", old, Context.Data);
		}
		
		/// <summary>
		/// This will return the tokens that this site currently has to the
		/// other participants, thus ensuring that they can propagate their
		/// changes back to us.
		/// </summary>
		public void FlushToken()
		{
			System.Console.WriteLine("{0} FlushTimer State: {1}", SiteId, State);
			if(State == State.HavingToken)
			{
				System.Console.WriteLine("{0} wants to Flush", SiteId);
				State = State.WaitingForToken;
				Connection.Send(new DiffMessage(""));
				System.Console.WriteLine("{0} flushing token", SiteId);
			}
		}
		
		/**
         * Merge a set of patches onto the text.  Return a patched text, as well
         * as an array of true/false values indicating which patches were applied.
         * @param patches Array of patch objects
         * @param text Old text.
         * @return Two element Object array, containing the new text and an array of
         *      bool values.
         */
        private Object[] patch_apply(List<Patch> patches, string text, int[] offsets)
        {
            if (patches.Count == 0)
            {
                return new Object[] { text, new bool[0] };
            }

            // Deep copy the patches so that no changes are made to originals.
            patches = DiffMatchPatch.patch_deepCopy(patches);

            string nullPadding = DiffMatchPatch.patch_addPadding(patches);
            text = nullPadding + text + nullPadding;
            DiffMatchPatch.patch_splitMax(patches);

            int x = 0;
            // delta keeps track of the offset between the expected and actual location
            // of the previous patch.  If there are patches expected at positions 10 and
            // 20, but the first patch was found at 12, delta is 2 and the second patch
            // has an effective expected position of 22.
            int delta = 0;
            bool[] results = new bool[patches.Count];
            foreach (Patch aPatch in patches)
            {
                int expected_loc = aPatch.start2 + delta;
                string text1 = DiffMatchPatch.diff_text1(aPatch.diffs);
                int start_loc;
                int end_loc = -1;
                if (text1.Length > DiffMatchPatch.Match_MaxBits)
                {
                    // patch_splitMax will only provide an oversized pattern
                    // in the case of a monster delete.
                    start_loc = DiffMatchPatch.match_main(text,
                        text1.Substring(0, DiffMatchPatch.Match_MaxBits), expected_loc);
                    if (start_loc != -1)
                    {
                        end_loc = DiffMatchPatch.match_main(text,
                            text1.Substring(text1.Length - DiffMatchPatch.Match_MaxBits),
                            expected_loc + text1.Length - DiffMatchPatch.Match_MaxBits);
                        if (end_loc == -1 || start_loc >= end_loc)
                        {
                            // Can't find valid trailing context.  Drop this patch.
                            start_loc = -1;
                        }
                    }
                }
                else
                {
                    start_loc = DiffMatchPatch.match_main(text, text1, expected_loc);
                }
                if (start_loc == -1)
                {
                    // No match found.  :(
                    results[x] = false;
                    // Subtract the delta for this failed patch from subsequent patches.
                    delta -= aPatch.length2 - aPatch.length1;
                }
                else
                {
                    // Found a match.  :)
                    results[x] = true;
                    delta = start_loc - expected_loc;
                    string text2;
                    if (end_loc == -1)
                    {
                        text2 = text.JavaSubstring(start_loc,
                            Math.Min(start_loc + text1.Length, text.Length));
                    }
                    else
                    {
                        text2 = text.JavaSubstring(start_loc,
                            Math.Min(end_loc + DiffMatchPatch.Match_MaxBits, text.Length));
                    }
                    if (text1 == text2)
                    {
                        // Perfect match, just shove the Replacement text in.
                        text = text.Substring(0, start_loc) + DiffMatchPatch.diff_text2(aPatch.diffs)
                            + text.Substring(start_loc + text1.Length);
                    }
                    else
                    {
                        // Imperfect match.  Run a diff to get a framework of equivalent
                        // indices.
                        List<Diff> diffs = DiffMatchPatch.diff_main(text1, text2, false);
                        if (text1.Length > DiffMatchPatch.Match_MaxBits
                            && DiffMatchPatch.diff_levenshtein(diffs) / (float) text1.Length
                            > DiffMatchPatch.Patch_DeleteThreshold)
                        {
                            // The end points match, but the content is unacceptably bad.
                            results[x] = false;
                        }
                        else
                        {
                            DiffMatchPatch.diff_cleanupSemanticLossless(diffs);
                            int index1 = 0;
                            foreach (Diff aDiff in aPatch.diffs)
                            {
                                if (aDiff.operation != Operation.EQUAL)
                                {
                                    int index2 = DiffMatchPatch.diff_xIndex(diffs, index1);
                                    if (aDiff.operation == Operation.INSERT)
                                    {
                                        // Insertion
                                        text = text.Insert(start_loc + index2, aDiff.text);
                                        
                                        // Adjust offset
                                        for(int i = 0; i < offsets.Length; ++i) 
                                        {
                                        	if(offsets[i] + nullPadding.Length > start_loc + index2)
                                        	{
                                        		offsets[i] += aDiff.text.Length;
                                        	}
                                        }
                                    }
                                    else if (aDiff.operation == Operation.DELETE)
                                    {
                                        // Deletion
                                        int del_start = start_loc + index2;
                                        int del_end = start_loc + DiffMatchPatch.diff_xIndex(diffs, index1 + aDiff.text.Length);
                                        text = text.Substring(0, del_start) + text.Substring(del_end);
                                       
                                        // Adjust offset
										for(int i = 0; i < offsets.Length; ++i) 
                                        {
                                        	if(offsets[i] + nullPadding.Length > del_start)
                                        	{
                                        		if(offsets[i] + nullPadding.Length < del_end)
                                        		{
                                        			offsets[i] = del_start - nullPadding.Length;
                                        		}
                                        		else
                                        		{
                                        			offsets[i] -= del_end - del_start;
                                        		}
                                        	}
                                        }                                        
                                    }
                                }
                                if (aDiff.operation != Operation.DELETE)
                                {
                                    index1 += aDiff.text.Length;
                                }
                            }
                        }
                    }
                }
                x++;
            }
            // Strip the padding off.
            text = text.JavaSubstring(nullPadding.Length, text.Length
                - nullPadding.Length);
            return new Object[] { text, results };
        }
	}
}
