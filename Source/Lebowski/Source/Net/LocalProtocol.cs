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
		
		public event EventHandler<ReceivedEventArgs> Received;

		public LocalConnection()
		{
			eventQueue = new Queue<ReceivedEventArgs>();
			
			// Start dispatcher thread
			ThreadStart threadStart = new ThreadStart(RunDispatcher);
			dispatcherThread = new Thread(threadStart);
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
				System.Console.WriteLine("Receive({0})", e.Message);
				Endpoint.OnReceived(e);
			}
		}
	}
}
