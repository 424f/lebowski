using System;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;

namespace Lebowski
{
	public interface ISessionContext
	{
		DifferentialSynchronizationStrategy SynchronizationStrategy { get; }
		IConnection Connection { get; }
		TextModel.ITextContext Context { get; }
		string FileName { get; }
		
		void StartSession(
			DifferentialSynchronizationStrategy strategy,
			IConnection connection,
			IConnection applicationConnection
		);
			
	}
}
