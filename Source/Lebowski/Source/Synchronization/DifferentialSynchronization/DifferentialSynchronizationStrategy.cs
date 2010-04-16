using System;
using System.Collections.Generic;
using Lebowski.TextModel;
using DiffMatchPatch;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
	public class DifferentialSynchronizationStrategy
	{
		public int SiteId { get; protected set; }
		ITextContext Context;
		List<ITextContext> Shadows;
		diff_match_patch DiffMatchPatch = new diff_match_patch();
		
		public DifferentialSynchronizationStrategy(ITextContext context)
		{
			DiffMatchPatch =  new diff_match_patch();
			Context = context;
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
