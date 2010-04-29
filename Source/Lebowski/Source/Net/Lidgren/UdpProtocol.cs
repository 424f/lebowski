using System;

namespace Lebowski.Net.Lidgren
{
	public class UdpProtocol : ICommunicationProtocol
	{
		public string Name
		{
			get { return "UDP"; }
		}
		
		public void Share(ISessionContext session)
		{
			throw new NotImplementedException();
		}
		
	}
}
