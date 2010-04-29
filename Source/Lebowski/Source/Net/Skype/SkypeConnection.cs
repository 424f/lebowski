using System;
using System.Text;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

namespace Lebowski.Net.Skype
{
	public class SkypeConnection : IConnection
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		public event EventHandler<ReceivedEventArgs> Received;

		private string remote;
		private int connectionId;
		private SkypeProtocol protocol;
		
		public SkypeConnection(SkypeProtocol protocol, string remote, int connectionId)
		{
			this.connectionId = connectionId;
			this.protocol = protocol;
			this.remote = remote;
			
			Send("HAI");
		}
		
		public void Send(object o)
		{
			protocol.Send(remote, connectionId, o);
		}
		
		internal virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
	}
}
