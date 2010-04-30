using System;
using System.Text;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

namespace Lebowski.Net.Skype
{
	public sealed class SkypeConnection : IConnection
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		public event EventHandler<ReceivedEventArgs> Received;

		private string remote;
		public int IncomingChannel { get; private set; }
		public int OutgoingChannel { get; set; }
		
		private SkypeProtocol protocol;
		
		public SkypeConnection(SkypeProtocol protocol, string remote, int incomingChannel)
		{
			this.IncomingChannel = incomingChannel;
			this.protocol = protocol;
			this.remote = remote;
			OutgoingChannel = -1;
			
			Send("HAI");
		}
		
		public void Send(object o)
		{
			if(OutgoingChannel == -1)
			{
				throw new InvalidOperationException("You have to define an outgoing channel before sending messages.");
			}
			protocol.Send(remote, IncomingChannel, o);
		}
		
		internal void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
	}
}
