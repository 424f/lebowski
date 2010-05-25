using System;
using System.Threading;
using System.Collections.Generic;

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
		
		private Thread dispatcherThread;
		private Queue<ReceivedEventArgs> eventQueue;
		
		public object Tag { get; set; }
		
		public event EventHandler<ReceivedEventArgs> Received;
		
		private int ReceiveDelay = 0;

		public LocalConnection()
		{
			eventQueue = new Queue<ReceivedEventArgs>();
			
			// Start dispatcher thread
			ThreadStart threadStart = new ThreadStart(RunDispatcher);
			dispatcherThread = new Thread(threadStart);
			dispatcherThread.Name = "LocalConnection Dispatcher Thread";
			dispatcherThread.Start();
		}
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
		public void Send(object message)
		{
			System.Console.WriteLine("Send({0})", message);
			lock(eventQueue)
			{
				eventQueue.Enqueue(new ReceivedEventArgs(message));
				Monitor.PulseAll(eventQueue);
			}
		}
		
		private void RunDispatcher()
		{
			bool running = true;
			while(running)
			{
				ReceivedEventArgs e = null;
				lock(eventQueue)
				{
					while(eventQueue.Count == 0)
					{
						Monitor.Wait(eventQueue);
					}
					e = eventQueue.Dequeue();
				}
				if(ReceiveDelay > 0)
				{
					Thread.Sleep(ReceiveDelay);
				}
				System.Console.WriteLine("Receive({0})", e.Message);
				Endpoint.OnReceived(e);
				lock(eventQueue)
				{
					Monitor.PulseAll(eventQueue);					
				}
			}
		}
		
		public void Close()
		{

		}
		
		/// <summary>
		/// Makes sure that all packets currently enqueued are dispatched,
		/// before the method returns. This is handy for writing tests, as
		/// we have to wait until the packets have actually been dispatched
		/// before we look at the context states.
		/// </summary>
		public void DispatchAll()
		{
			lock(eventQueue)
			{
				while(eventQueue.Count > 0)
				{
					Monitor.Wait(eventQueue);
				}
			}
		}
	}
}
