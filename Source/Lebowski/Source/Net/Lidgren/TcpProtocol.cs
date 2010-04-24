using System;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
		public const int Port = 12345;
		
		public event EventHandler<ReceivedEventArgs> Received;	
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
		
		protected void SerializeToBuffer(object o, NetBuffer buffer)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, o);
			ms.Close();
			buffer.Write(ms.ToArray());
		}
		
		protected object DeserializeFromBuffer(NetBuffer buffer)
		{
			MemoryStream ms = new MemoryStream();
			int length = buffer.LengthBytes;
			ms.Write(buffer.ReadBytes(length), 0, length);
			ms.Seek(0, SeekOrigin.Begin);
			
			BinaryFormatter formatter = new BinaryFormatter();
			return formatter.Deserialize(ms);
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
			SerializeToBuffer(o, buffer);
			Socket.SendMessage(buffer, NetChannel.ReliableInOrder1);
		}
		
	}
	
	public class ServerConnection : LidgrenConnection
	{
		NetServer Socket;
		NetConnection Client;
		
		public ServerConnection()
		{
			NetConfiguration config = new NetConfiguration(AppName);
			config.MaxConnections = 1;
			config.Port = Port;
			Socket = new NetServer(config);
			
			// Create networking thread
			ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
			Thread thread = new Thread(threadStart);
			thread.Start();			
		}
		
		private void RunNetworkingThread()
		{
			Socket.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
			Socket.Start();
			
			NetBuffer buffer = Socket.CreateBuffer();
			
			bool running = true;
			while(running)
			{
				NetMessageType type;
				NetConnection sender;
				
				while(Socket.ReadMessage(buffer, out type, out sender))
				{
					switch(type)
					{
						case NetMessageType.ConnectionApproval:
							sender.Approve();
							break;
						
						case NetMessageType.StatusChanged:
							break;
						
						case NetMessageType.Data:
							Object message = DeserializeFromBuffer(buffer);
							OnReceived(new ReceivedEventArgs(message));
							break;							
					}
				}
				
				Thread.Sleep(1);
			}
		}
		
		public override void Send(object o)
		{
			NetBuffer buffer = Socket.CreateBuffer();
			SerializeToBuffer(o, buffer);
			Socket.SendToAll(buffer, NetChannel.ReliableInOrder1);
		}
		
	}
	
}
