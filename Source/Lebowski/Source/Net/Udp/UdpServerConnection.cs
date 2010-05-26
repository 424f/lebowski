namespace Lebowski.Net.Udp
{
    using System;
    using System.Threading;
    using log4net;
    using Lidgren.Network;
    
    public sealed class ServerConnection : UdpConnection
    {
        NetServer Socket;
        //NetConnection Client;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UdpConnection));

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
