using System;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
	[Serializable]
	internal sealed class DiffMessage
	{
		public string Diff { get; private set; }
		
		public DiffMessage(string diff)
		{
			Diff = diff;
		}
		
		public override string ToString()
		{
			return String.Format("DiffMessage({0})", Diff);
		}
	}
}
