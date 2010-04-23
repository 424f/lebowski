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
	
	[Serializable]
	/// <summary>
	/// This message is sent, when a client would like to send its updates, but doesn't currently hold
	/// the token.
	/// </summary>
	class TokenRequestMessage
	{
		public override string ToString()
		{
			return "TokenRequestMessage()";
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
		public bool TokenRequestSent = false;
		
		public DifferentialSynchronizationStrategy(int siteId, ITextContext context, IConnection connection)
		{
			SiteId = siteId;
			DiffMatchPatch =  new diff_match_patch();
			Context = context;
			Connection = connection;
			Shadow = new StringTextContext();
			
			HasChanged = false;
			
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
						TokenRequestSent = false;
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
					}
					else if(e.Message is TokenRequestMessage)
					{
						TokenRequestMessage message = (TokenRequestMessage)e.Message;
						FlushToken();
					}
					else 
					{
						throw new Exception(String.Format("Encountered unknown message type '{0}'", e.GetType().Name));
					}
				}
			};
			
			Context.Changed += delegate(object sender, ChangeEventArgs e)
			{
				System.Console.WriteLine("{0}'s context changed by {1}", SiteId, e.Issuer);
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
						if(!TokenRequestSent)
						{
							Connection.Send(new TokenRequestMessage());
						}
						TokenRequestSent = true;
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
			Context.Invoke((Action)delegate {
			    Console.WriteLine("Selection was: {0} {1}", Context.SelectionStart, Context.SelectionEnd);
				int[] offsets = {Context.SelectionStart, Context.SelectionEnd};
				Context.Data = (string)DiffMatchPatch.patch_apply(textPatch, Context.Data)[0];			
				
				// We restore the offset locations using absolute referencing
				// See: http://neil.fraser.name/writing/cursor/
				foreach(Patch patch in textPatch)
				{
					foreach(Diff diff in patch.diffs)
					{
						int index = 0;
						switch(diff.operation)
						{
							case Operation.DELETE:
								for(int i = 0; i < offsets.Length; ++i)
								{
									if(offsets[i] > index) 
									{
										offsets[i] -= diff.text.Length;
									}
								}								
								break;
							case Operation.EQUAL:
								index += diff.text.Length;								
								break;
							case Operation.INSERT:
								for(int i = 0; i < offsets.Length; ++i)
								{
									if(offsets[i] > index) 
									{
										offsets[i] += diff.text.Length;
									}
								}
								index += diff.text.Length;
								break;
						}
					}
				}
				Context.SetSelection(offsets[0], offsets[1]);			               	
				Console.WriteLine("Selection is: {0} {1}", offsets[0], offsets[1]);
				Console.WriteLine("Was {0} now {1}", old, Context.Data);
			});
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
	}
}
