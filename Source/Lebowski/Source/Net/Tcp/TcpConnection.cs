using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lebowski.Net;

namespace Lebowski.Net.Tcp
{
	public abstract class TcpConnection : IConnection
	{
		protected const int Port = 12345;
		public event EventHandler<ReceivedEventArgs> Received;	
		protected NetworkStream stream;
		static int activeConnections = 0;
		
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
			bool running = true;
			while(running)
			{
				// First, read the package length
				int packetLength = BitConverter.ToInt32(NetUtils.ReadBytes(stream, 4), 0);
				byte[] packet = NetUtils.ReadBytes(stream, packetLength);
				object message = NetUtils.Deserialize(packet);
				OnReceived(new ReceivedEventArgs(message));
			}			
		}
		
		public void Send(object o)
		{
			// TODO: might have to be made thread-safe
			byte[] packet = NetUtils.Serialize(o);
			byte[] packetWithHeader = new byte[packet.Length + 4];
			BitConverter.GetBytes(packet.Length).CopyTo(packetWithHeader, 0);
			packet.CopyTo(packetWithHeader, 4);
			stream.Write(packetWithHeader, 0, packetWithHeader.Length);
			stream.Flush();
		}		
	}
	
	public class TcpServerConnection : TcpConnection
	{		
		private TcpListener tcpListener;
		private TcpClient client;
		
		public TcpServerConnection()
		{
			tcpListener = new TcpListener(IPAddress.Any, Port);
			
			// Create networking thread
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Start();				
		}
		
		protected void RunNetworkingThread()
		{
			tcpListener.Start();
			// TODO: handle multiple clients
			client = tcpListener.AcceptTcpClient();
			stream = client.GetStream();	
			
			RunReceiveThread();
		}		
		
	}
	
	public class TcpClientConnection : TcpConnection
	{
		TcpClient client;
		
		public TcpClientConnection(string address)
		{
			client = new TcpClient();
			IPHostEntry hostEntry = Dns.GetHostEntry(address);
			IPEndPoint endpoint = new IPEndPoint(hostEntry.AddressList[0], Port);
			client.Connect(endpoint);
			
			
			// Create networking thread
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Start();				
		}
		
		protected void RunNetworkingThread()
		{
			stream = client.GetStream();			
			RunReceiveThread();
		}
	}
}
