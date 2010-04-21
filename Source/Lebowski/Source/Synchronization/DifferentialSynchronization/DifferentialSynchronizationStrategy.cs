using System;
using System.Collections.Generic;
using Lebowski.TextModel;
using Lebowski.Net;
using DiffMatchPatch;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
	enum State
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
		State state;
		
		public DifferentialSynchronizationStrategy(int siteId, ITextContext context, IConnection connection)
		{
			SiteId = siteId;
			DiffMatchPatch =  new diff_match_patch();
			Context = context;
			Connection = connection;
			
			if(siteId == 0)
			{
				state = State.HavingToken;
			}
			else
			{
				state = State.WaitingForToken;
			}
				
			Connection.Received += delegate(object sender, ReceivedEventArgs e)
			{
				if(e.Message is DiffMessage)
				{
					DiffMessage diffMessage = (DiffMessage)e.Message;
					ApplyPatches(diffMessage.Diff);
					
				}
			};
			
			Context.Changed += delegate(object sender, EventArgs e)
			{
				if(state == State.HavingToken)
				{
					SendPatches();
					hasChanged = false;
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
		
		public void ReceivePatches(int remoteSite, List<Patch> patches)
		{
			Shadow.Data = (string)DiffMatchPatch.patch_apply(patches, Context.Data)[0];
			Context.Data = (string)DiffMatchPatch.patch_apply(patches, Context.Data)[0];
		}
		
		void ApplyPatches(string delta)
		{
			// Apply at remote host
			var diffs = DiffMatchPatch.diff_fromDelta(Shadow.Data, delta);
			var shadowPatch = DiffMatchPatch.patch_make(Shadow.Data, diffs);
			Shadow.Data = (string)DiffMatchPatch.patch_apply(shadowPatch, Shadow.Data)[0];
			var textPatch = DiffMatchPatch.patch_make(Context.Data, diffs);
			Context.Data = (string)DiffMatchPatch.patch_apply(textPatch, Context.Data)[0];			
		}
	}
}
