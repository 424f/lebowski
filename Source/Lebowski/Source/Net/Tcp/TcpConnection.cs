namespace Lebowski.Net.Tcp
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Lebowski.Net;
    using log4net;

    /// <summary>
    /// A connection using TCP with a fixed endpoint.
    /// </summary>
    public abstract class TcpConnection : AbstractConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TcpConnection));        
        
        /// <summary>
        /// The NetworkStream that is used to deliver data to the endpoint
        /// </summary>
        protected NetworkStream stream;
        
        /// <summary>
        /// Indicates whether the receive thread should keep running
        /// </summary>
        private bool running = true;

        /// <summary>
        /// Runs a receive thread, retrieving packets from the network and 
        /// dispatching them using the <see cref="Lebowski.Net.IConnection.Received">Received</see> event.
        /// </summary>
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

        /// <inheritdoc/>
        public override void Send(object o)
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

        /// <inheritdoc/>
        public override void Close()
        {

            Logger.InfoFormat("Closing stream: {0}", stream);
            try
            {
                if(stream != null)
                {
                    stream.Close();
                }
            }
            finally
            {
                running = false;
                OnConnectionClosed(new EventArgs());
            }
        }
    }
}