using System;

namespace Lebowski.Net.Tcp
{
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    
    /// <summary>
    /// A TCP server that listens to incoming connections and accepts
    /// the first one, then providing methods to communicate with it.
    /// </summary>
    public class TcpServerConnection : TcpConnection
    {
        private TcpListener tcpListener;
        private TcpClient tcpClient;

        /// <summary>
        /// Initializes a new instance of the TcpServerConnection, listening
        /// to connections on the specified port.
        /// </summary>
        /// <param name="port">The TCP port to listen for a connection.</param>
        public TcpServerConnection(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            
            // Create networking thread            
            ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
            Thread thread = new Thread(threadStart);
            thread.Name = "TcpServerConnection Thread";
            thread.Start();
        }

        /// <inheritdoc/>
        public override void Close()
        {
            tcpListener.Stop();
            tcpClient.Client.Close();
            base.Close();
        }

        /// <summary>
        /// Runs a networking thread, accepting a single connection and then
        /// behaving like <see cref="TcpConnection.RunReceiveThread">RunReceiveThread</see>.
        /// </summary>
        protected void RunNetworkingThread()
        {
            // TODO: handle multiple clients
            // waits for incoming client connection
            tcpClient = tcpListener.AcceptTcpClient();
            // stops listening when first client connection has been accepted
            tcpListener.Stop();
            stream = tcpClient.GetStream();
            OnClientConnected(new EventArgs());
            RunReceiveThread();
        }

        /// <summary>
        /// Raises the <see cref="ClientConnected">ClientConnected</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnClientConnected(EventArgs e)
        {
            if (ClientConnected != null)
            {
                ClientConnected(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ClientDisconnected">ClientDisconnected</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnClientDisconnected(EventArgs e)
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, e);
            }
        }

        /// <summary>
        /// Occurs when a client connects to this server.
        /// </summary>
        public event EventHandler<EventArgs> ClientConnected;
        
        /// <summary>
        /// Occurs when a client disconnects from this server.
        /// </summary>
        public event EventHandler<EventArgs> ClientDisconnected;        

    }
}
