
using System;

namespace Lebowski.Net
{
	public interface ICommunicationProtocol
	{
		string Name { get; }
		void Share(ISessionContext session);
		
		bool CanShare { get; }
		bool CanParticipate { get; }
		
		event EventHandler<HostSessionEventArgs> HostSession;
		event EventHandler<JoinSessionEventArgs> JoinSession;
	}
	
	public sealed class HostSessionEventArgs : EventArgs
	{
		public IConnection Connection { get; private set; }
		public IConnection ApplicationConnection { get; private set; }
		public ISessionContext Session { get; private set; }
		
		public HostSessionEventArgs(ISessionContext session, IConnection connection, IConnection applicationConnection)
		{
			Session = session;
			Connection = connection;
			ApplicationConnection = applicationConnection;
		}
	}
	
	public sealed class JoinSessionEventArgs : EventArgs
	{
		public IConnection Connection { get; private set; }
		public IConnection ApplicationConnection { get; private set; }
		
		public JoinSessionEventArgs(IConnection connection, IConnection applicationConnection)
		{
			Connection = connection;
			ApplicationConnection = applicationConnection;
		}
	}
	
}
