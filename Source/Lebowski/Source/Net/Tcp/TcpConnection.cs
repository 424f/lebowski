namespace Lebowski.Net.Tcp
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Lebowski.Net;
    using log4net;

    public abstract class TcpConnection : IConnection
    {
        /// <summary>
        /// Occurs when the connection has been closed
        /// </summary>
        public event EventHandler<EventArgs> ConnectionClosed;
        public event EventHandler<ReceivedEventArgs> Received;
        protected NetworkStream stream;
        private bool running = true;

        public object Tag { get; set; }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnection));

        protected virtual void OnReceived(ReceivedEventArgs e)
        {
            if (Received != null) {
                Received(this, e);
            }
        }

        protected void RunReceiveThread()
        {
            running = true;
            try
            {
                while (running)
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
                        Logger.ErrorFormat("Encountered error while dispatching message: {0}", e);
                    }
                    // TODO: handle disconnection
                }
            }
            catch(Exception e)
            {
                Logger.ErrorFormat("An error occurred in the receiver thread, closing connection: {0}", e.ToString());
            }
            finally
            {
                Close();
            }
        }

        public void Send(object o)
        {
            Logger.InfoFormat("Sending packet {2} on stream from thread '{0}' #{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId, o.GetType().Name);

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
                    Logger.ErrorFormat("An error occurred during Send: {0}", e);
                    throw e;
                }
            }
        }

        public virtual void Close()
        {

            Logger.InfoFormat("Closing stream: {0}", stream);
            try
            {
                stream.Close();
            }
            finally
            {
                running = false;
                OnConnectionClosed(new EventArgs());
            }
        }

        protected virtual void OnConnectionClosed(EventArgs e)
        {
            if (ConnectionClosed != null) {
                ConnectionClosed(this, e);
            }
        }
    }
}