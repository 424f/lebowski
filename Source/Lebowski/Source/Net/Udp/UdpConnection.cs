using Lidgren.Network;

namespace Lebowski.Net.Udp
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using log4net;
    using Lebowski.Net;

    public abstract class LidgrenConnection : AbstractConnection
    {
        public const string AppName = "LEBOWSKI";
        public const int Port = 12345;

        protected void SerializeToBuffer(object o, NetBuffer buffer)
        {
            buffer.Write(NetUtils.Serialize(o));
        }

        protected object DeserializeFromBuffer(NetBuffer buffer)
        {
            return NetUtils.Deserialize(buffer.ReadBytes(buffer.LengthBytes));
        }

        public override abstract void Send(object o);

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class ServerConnection : LidgrenConnection
    {
        NetServer Socket;
        //NetConnection Client;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LidgrenConnection));

        public ServerConnection()
        {
            NetConfiguration config = new NetConfiguration(AppName);
            config.MaxConnections = 32; // TODO: only one connection
            config.Port = Port;
            Socket = new NetServer(config);

            // Create networking thread
            ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
            Thread thread = new Thread(threadStart);
            thread.Name = "Udp Networking Thread (Server)";
            thread.Start();
        }

        private void RunNetworkingThread()
        {
            Socket.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
            Socket.Start();

            NetBuffer buffer = Socket.CreateBuffer();

            bool running = true;
            while (running)
            {
                NetMessageType type;
                NetConnection sender;

                while (Socket.ReadMessage(buffer, out type, out sender))
                {
                    switch(type)
                    {
                        case NetMessageType.ConnectionApproval:
                            Logger.Info(String.Format("Accepted connection by {0}", sender));
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