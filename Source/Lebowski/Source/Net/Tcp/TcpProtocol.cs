using System;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace Lebowski.Net.Tcp
{
	public class TcpProtocol : ICommunicationProtocol
	{
		public event EventHandler<HostSessionEventArgs> HostSession;
		public event EventHandler<JoinSessionEventArgs> JoinSession;
		
		public string Name
		{
			get { return "TCP"; }
		}
		
		public void Share(ISessionContext session)
		{
			TcpServerConnection connection = new TcpServerConnection();	
			
			// We have to use a multichannel connection
			MultichannelConnection mcc = new MultichannelConnection(connection);
			var sync = new DifferentialSynchronizationStrategy(0, session.Context, mcc.CreateChannel());
			var applicationChannel = mcc.CreateChannel();		
			session.StartSession(sync, connection, applicationChannel);
		}		
		
		public bool CanShare
		{
			get { return true; }
		}
		
		public bool CanParticipate
		{
			get { return true; }
		}		
	}
}
