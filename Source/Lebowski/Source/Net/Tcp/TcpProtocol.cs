using System;

namespace Lebowski.Net.Tcp
{
	public class TcpProtocol : ICommunicationProtocol
	{
		public string Name
		{
			get { return "TCP"; }
		}
	}
}
