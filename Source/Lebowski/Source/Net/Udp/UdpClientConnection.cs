namespace Lebowski.Net.Udp
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Lidgren.Network;
    using log4net;
    
    public sealed class UdpClientConnection : UdpConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UdpConnection));

        NetClient Socket;
        bool Running = true;

        string address;
        int port;

        public UdpClientConnection(string address, int port)
        {
            this.address = address;
            this.port = port;

            NetConfiguration config = new NetConfiguration(AppName);
            Socket = new NetClient(config);
            Socket.SetMessageTypeEnabled(NetMessageType.ConnectionRejected, true);
            Socket.SetMessageTypeEnabled(NetMessageType.DebugMessage, true);

            Socket.Connect(address, port, new byte[] { 123, 123 } );

            // Create networking thread
            ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
            Thread thread = new Thread(threadStart);
            thread.Name = "Udp Networking Thread";
            thread.Start();
        }

        private void RunNetworkingThread()
        {
            NetBuffer buffer = Socket.CreateBuffer();
            while (Running)
            {
                NetMessageType messageType;
                while (Socket.ReadMessage(buffer, out messageType))
                {
                    switch(messageType)
                    {
                        case NetMessageType.Data:
                            Object message = DeserializeFromBuffer(buffer);
                            OnReceived(new ReceivedEventArgs(message));
                            break;

                        case NetMessageType.ConnectionRejected:
                            Logger.Info("Your connection was rejected.");
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
}
