
using System;

namespace Lebowski.Net
{
	public class LocalProtocol
	{
		public static void Connect(LocalConnection first, LocalConnection second)
		{
			first.Endpoint = second;
			second.Endpoint = first;
		}
	}
	
	public class LocalConnection : IConnection
	{
		public LocalConnection Endpoint { set; protected get; }
		
		public event EventHandler<ReceivedEventArgs> Received;
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
		public void Send(object message)
		{
			Endpoint.OnReceived(new ReceivedEventArgs(message));
		}
	}
}
