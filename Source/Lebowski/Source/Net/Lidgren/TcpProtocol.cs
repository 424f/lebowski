using System;
using System.Threading;
using Lidgren.Network;

namespace Lebowski.Net.Lidgren
{
	public class TcpProtocol
	{
		public TcpProtocol()
		{
		}
	}
	
	public abstract class LidgrenConnection : IConnection
	{
		public const string AppName = "LEBOWSKI";
		
		public event EventHandler<ReceivedEventArgs> Received;	
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
		
		protected void SerializeToBuffer(object o, NetBuffer buffer)
		{
			
		}
		
		protected object DeserializeFromBuffer(NetBuffer buffer)
		{
			return null;
		}
		
		public abstract void Send(object o);
	}
	
	public class ClientConnection : LidgrenConnection
	{
		NetClient Socket;
		bool Running = true;
		
		public ClientConnection(string address, int port)
		{
			NetConfiguration config = new NetConfiguration(AppName);
			Socket = new NetClient(config);
			Socket.Connect(address, port);
			
			// Create networking thread
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Start();
		}
		
		private void RunNetworkingThread()
		{
			NetBuffer buffer = Socket.CreateBuffer();
			while(Running)
			{
				NetMessageType messageType;
				while(Socket.ReadMessage(buffer, out messageType))
				{
					switch(messageType)
					{
						case NetMessageType.Data:
							Object message = DeserializeFromBuffer(buffer);
							OnReceived(new ReceivedEventArgs(message));
							break;
							
						default:
							break;
					}
				}
			}
		}
		
		public override void Send(object o)
		{
			NetBuffer buffer = Socket.CreateBuffer();
			Socket.SendMessage(buffer, NetChannel.ReliableInOrder1);
		}
		
	}
	
	public class ServerConnection : LidgrenConnection
	{
		NetServer Socket;
		NetConnection Client;
		
		public override void Send(object o)
		{
			NetBuffer buffer = Socket.CreateBuffer();
			Socket.SendMessage(buffer, Client, NetChannel.ReliableInOrder1);
		}
		
	}
	
}
