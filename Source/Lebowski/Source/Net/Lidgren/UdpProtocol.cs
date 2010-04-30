using System;

namespace Lebowski.Net.Lidgren
{
	public class UdpProtocol : ICommunicationProtocol
	{
		public string Name
		{
			get { return "UDP"; }
		}
		
		public bool CanShare
		{
			get { return true; }
		}
		
		public bool CanParticipate
		{
			get { return true; }
		}
		
		public void Share(ISessionContext session)
		{
			throw new NotImplementedException();
		}
		
		public event EventHandler<HostSessionEventArgs> HostSession;
		public event EventHandler<JoinSessionEventArgs> JoinSession;

	}
}
