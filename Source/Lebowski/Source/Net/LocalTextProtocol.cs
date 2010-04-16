
using System;

namespace Lebowski.Net
{
	/// <summary>
	/// A local text protocol for testing, that just redirects the packets
	/// from one site to the other
	/// </summary>
	public class LocalTextProtocol
	{
		public static void Connect(LocalTextConnection first, LocalTextConnection second)
		{
			first.Other = second;
			second.Other = first;
		}
	}
	
	public class LocalTextConnection : ITextConnection
	{
		public LocalTextConnection Other { get; set; }
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		public event EventHandler<ReceivedEventArgs> Received;
		
		public void Send(string message)
		{
			Other.OnReceived(new ReceivedEventArgs(message));
		}
		
	}
}
