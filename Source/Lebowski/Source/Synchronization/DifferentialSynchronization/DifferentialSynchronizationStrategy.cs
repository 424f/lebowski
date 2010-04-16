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
	
	public class DifferentialSynchronizationStrategy
	{
		public int SiteId { get; protected set; }
		ITextConnection Connection;
		ITextContext Context;
		List<ITextContext> Shadows;
		diff_match_patch DiffMatchPatch = new diff_match_patch();
		
		bool hasChanged = false;
		State state;
		
		public DifferentialSynchronizationStrategy(bool isServer, ITextContext context, ITextConnection connection)
		{
			DiffMatchPatch =  new diff_match_patch();
			Context = context;
			Connection = connection;
			
			if(isServer)
			{
				state = State.HavingToken;
			}
			else
			{
				state = State.WaitingForToken;
			}
				
			
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
			
		}
		
		public void ReceivePatches(int remoteSite, List<Patch> patches)
		{
			Shadows[remoteSite].Data = (string)DiffMatchPatch.patch_apply(patches, Context.Data)[0];
			Context.Data = (string)DiffMatchPatch.patch_apply(patches, Context.Data)[0];
		}
		
		void ApplyPatches(List<Patch> patches, ITextContext context)
		{
			foreach(Patch patch in patches)
			{
				
			}
			context.Data = (string)DiffMatchPatch.patch_apply(patches, context.Data)[0];
		}
	}
}
