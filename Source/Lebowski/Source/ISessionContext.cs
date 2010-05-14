using System;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;

namespace Lebowski
{
	public interface ISessionContext
	{
	    SessionState State { get; }
	    DifferentialSynchronizationStrategy SynchronizationStrategy { get; }
		TextModel.ITextContext Context { get; }
		string FileName { get; }
		
		void StartSession(
			DifferentialSynchronizationStrategy strategy,
			IConnection applicationConnection
		);
		
		void CloseSession();
			
	}
}
