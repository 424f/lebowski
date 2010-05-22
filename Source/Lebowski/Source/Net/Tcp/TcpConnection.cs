using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lebowski.Net;

namespace Lebowski.Net.Tcp
{
	public class ConnectionFailedException : Exception
	{
		public ConnectionFailedException(string message) : base(message) {}
		public ConnectionFailedException(string message, Exception cause) : base(message, cause) {}
	}	
	
	public abstract class TcpConnection : IConnection
	{
		/// <summary>
		/// Occurs when the connection has been closed 
		/// </summary>
		public event EventHandler<EventArgs> ConnectionClosed;
		
		public event EventHandler<ReceivedEventArgs> Received;	
		protected NetworkStream stream;
		static int activeConnections = 0;
		private bool running = true;
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			lock(this.GetType())
			{
				activeConnections += 1;
				if(activeConnections > 1)
				{
					Console.Error.WriteLine("Multiple active connections!!");
				}
			}
			if (Received != null) {
				Received(this, e);
			}
			lock(this.GetType())
			{
				activeConnections -= 1;
			}
		}		
		
		protected void RunReceiveThread()
		{
			running = true;
			try
			{
				while(running)
				{
					// First, read the packet length
					int packetLength = BitConverter.ToInt32(NetUtils.ReadBytes(stream, 4), 0);
					byte[] packet = NetUtils.ReadBytes(stream, packetLength);
					object message = NetUtils.Deserialize(packet);
					try
					{
					    OnReceived(new ReceivedEventArgs(message));						
					}
					catch(Exception e)
					{
					    System.Console.WriteLine("Encountered error while dispatching message: {0}", e);
					}
				}	
			}
			catch(Exception e)
			{
				// TODO: log
				System.Console.WriteLine("**ERROR** {0}", e.ToString());
			}
			finally
			{
				OnConnectionClosed(new EventArgs());
				Close();
			}
		}
		
		public void Send(object o)
		{
		    Console.WriteLine("Sending packet {2} on stream from thread '{0}' #{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId, o.GetType().Name);
		    
		    lock(this)
		    {
    			try
    			{
        			byte[] packet = NetUtils.Serialize(o);
        			byte[] packetWithHeader = new byte[packet.Length + 4];
        			BitConverter.GetBytes(packet.Length).CopyTo(packetWithHeader, 0);
        			packet.CopyTo(packetWithHeader, 4);
        			stream.Write(packetWithHeader, 0, packetWithHeader.Length);
        			stream.Flush();
    			}
    			catch(Exception e)
    			{
    			    Console.WriteLine("ERROR!!");
    			    throw e;
    			}
		    }
		}		
		
		public virtual void Close()
		{
		    System.Console.WriteLine("Closing stream.");
			stream.Close();
			running = false;
		}
		
		protected virtual void OnConnectionClosed(EventArgs e)
		{
			if (ConnectionClosed != null) {
				ConnectionClosed(this, e);
			}
		}			
	}
	
	public class TcpServerConnection : TcpConnection
	{	
		public event EventHandler<EventArgs> ClientConnected;
		public event EventHandler<EventArgs> ClientDisconnected;
		
		private TcpListener tcpListener;
		private TcpClient tcpClient;
		
		public TcpServerConnection(int port)
		{
			tcpListener = new TcpListener(IPAddress.Any, port);
			
			// Create networking thread
//			RunAsyncNetworkingThread();
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Name = "TcpServerConnection Thread";
			thread.Start();				
		}
		
		public override void Close()
		{
			tcpListener.Stop();
			base.Close();
		}
		
		protected void RunNetworkingThread()
		{
			tcpListener.Start();
			// TODO: handle multiple clients
			// waits for incoming client connection
			tcpClient = tcpListener.AcceptTcpClient();
			// stops listening when first client connection has been accepted 
			tcpListener.Stop();
			stream = tcpClient.GetStream();
			OnClientConnected(new EventArgs());
			RunReceiveThread();	
		}
		
		protected void RunAsyncNetworkingThread()
		{
			tcpListener.Start();
			// TODO: handle multiple clients
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), tcpListener);
		}
		
		/// <summary>
		/// AsyncCallback passed on BeginAcceptTcpClient to avoid the application to block when waiting for a client connection
		/// </summary>
		protected void DoAcceptTcpClientCallback(IAsyncResult ar)
		{
			tcpListener = (TcpListener) ar.AsyncState;
			tcpClient = tcpListener.EndAcceptTcpClient(ar);
			tcpListener.Stop();
			stream = tcpClient.GetStream();
			OnClientConnected(new EventArgs());
			RunReceiveThread();
		}
		
		protected virtual void OnClientConnected(EventArgs e)
		{
			if (ClientConnected != null)
			{
				ClientConnected(this, e);
			}
		}
		
		protected virtual void OnClientDisconnected(EventArgs e)
		{
			if (ClientDisconnected != null)
			{
				ClientDisconnected(this, e);
			}
		}
	}
	
	public class TcpClientConnection : TcpConnection
	{			
		TcpClient client;
		
		public TcpClientConnection(string address, int port)
		{
			try
			{
				client = new TcpClient();
				IPHostEntry hostEntry = Dns.Resolve(address);
				IPEndPoint endpoint = new IPEndPoint(hostEntry.AddressList[0], port);
				client.Connect(endpoint);
			}
			catch(Exception e)
			{
				throw new ConnectionFailedException(string.Format("Could not connect to remote host {0}:{1}", address, port), e);
			}
			
			// Create networking thread
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Name = "TcpClientConnection Thread";
			thread.Start();				
		}
		
		protected void RunNetworkingThread()
		{
			stream = client.GetStream();			
			RunReceiveThread();
		}		
	}
}
