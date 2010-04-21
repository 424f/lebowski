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
	}
	
	public class DifferentialSynchronizationStrategy
	{
		public int SiteId { get; protected set; }
		IConnection Connection;
		ITextContext Context;
		StringTextContext Shadow;
		diff_match_patch DiffMatchPatch = new diff_match_patch();
		
		bool hasChanged = false;
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
			
			EnableAutoFlush = false;
			FlushTimer = new Timer(10);
			FlushTimer.AutoReset = false;
			FlushTimer.Elapsed += delegate { FlushToken(); };			
			
			if(siteId == 0)
			{
				State = State.HavingToken;
			}
			else
			{
				State = State.WaitingForToken;
			}
				
			Connection.Received += delegate(object sender, ReceivedEventArgs e)
			{
				if(e.Message is DiffMessage)
				{
					State = State.HavingToken;
					DiffMessage diffMessage = (DiffMessage)e.Message;
					if(diffMessage.Diff != "") 
					{
						ApplyPatches(diffMessage.Diff);
					}
					
					if(hasChanged)
					{
						hasChanged = false;
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
			};
			
			Context.Changed += delegate(object sender, ChangeEventArgs e)
			{
				if(e.Issuer == this)
					return;
				if(State == State.HavingToken)
				{
					hasChanged = false;
					State = State.WaitingForToken;					
					SendPatches();
				}
				else
				{
					hasChanged = true;
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
			
			// Apply at remote host
			var diffs = DiffMatchPatch.diff_fromDelta(Shadow.Data, delta);
			var shadowPatch = DiffMatchPatch.patch_make(Shadow.Data, diffs);
			Shadow.Data = (string)DiffMatchPatch.patch_apply(shadowPatch, Shadow.Data)[0];
			var textPatch = DiffMatchPatch.patch_make(Context.Data, diffs);
			Context.Data = (string)DiffMatchPatch.patch_apply(textPatch, Context.Data)[0];			
			
			Console.WriteLine("Was {0} now {1}", old, Context.Data);
		}
		
		/// <summary>
		/// This will return the tokens that this site currently has to the
		/// other participants, thus ensuring that they can propagate their
		/// changes back to us.
		/// </summary>
		public void FlushToken()
		{
			if(State == State.HavingToken)
			{
				State = State.WaitingForToken;
				Connection.Send(new DiffMessage(""));
			}
		}
	}
}
