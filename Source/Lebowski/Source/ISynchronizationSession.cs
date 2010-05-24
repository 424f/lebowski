using System;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;

namespace Lebowski
{
	public interface ISynchronizationSession
	{
	    SessionState State { get; }
	    DifferentialSynchronizationStrategy SynchronizationStrategy { get; }
		TextModel.ITextContext Context { get; }
		
		void StartSession(
			DifferentialSynchronizationStrategy strategy,
			IConnection applicationConnection
		);
		
		void AwaitingSession();
		
		void CloseSession();
		
        string FileName { get; set; }
			
	}
}
