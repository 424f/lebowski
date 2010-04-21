
using System;

namespace Lebowski.Net
{
	public interface IConnection
	{
		void Send(object o);
		
		event EventHandler<ReceivedEventArgs> Received;
	}
	
	public class ReceivedEventArgs : EventArgs
	{
		public object Message { get; protected set; }
		
		public ReceivedEventArgs(object message)
		{
			Message = message;
		}
	}	
}
