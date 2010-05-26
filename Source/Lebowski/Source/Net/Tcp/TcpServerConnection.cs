using System;

namespace Lebowski.Net.Tcp
{
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    
    public class TcpServerConnection : TcpConnection
    {
        public event EventHandler<EventArgs> ClientConnected;
        public event EventHandler<EventArgs> ClientDisconnected;

        private TcpListener tcpListener;
        private TcpClient tcpClient;

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

        public override void Close()
        {
            tcpListener.Stop();
            base.Close();
        }

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
}
