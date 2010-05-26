using System;

namespace Lebowski.Net.Tcp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using log4net;
    
    /// <summary>
    /// A connection with a TCP server as the endpoint.
    /// </summary>
    public sealed class TcpClientConnection : TcpConnection
    {
        private static ILog Logger = LogManager.GetLogger(typeof(TcpConnection));
        private TcpClient client;

        /// <summary>
        /// Initializes a new instance of the TcpClientConnection class, trying
        /// to connect to a TCP server at the given port and address.
        /// </summary>
        /// <param name="address">The IP address or hostname of the server.</param>
        /// <param name="port">The port the server is listening to.</param>
        public TcpClientConnection(string address, int port)
        {
            try
            {
                client = new TcpClient();
                
                // Retrieve host host entry for address
                IPHostEntry hostEntry = Dns.GetHostEntry(address);
                
                // Try to connect on each endpoint
                string attempted = "";
                bool connected = false;
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    try
                    {
                        IPEndPoint endpoint = new IPEndPoint(ip, port);
                        attempted += ip.ToString() + "\n";
                        client.Connect(endpoint);
                        connected = true;
                        break;
                    }
                    catch(Exception e)
                    {
                        Logger.InfoFormat("Connecting to endpoint {0} failed: {1}", ip, e);
                    }
                }

                if (!connected)
                {
                    throw new Exception(string.Format("Could not connect to any of the following endpoints:\n{0}", attempted));
                }
            }
            catch(Exception e)
            {
                throw new ConnectionFailedException(string.Format("Could not connect to remote host {0}:{1}", address, port), e);
            }

            // Create networking thread
            ThreadStart threadStart = new ThreadStart(RunNetworkingThread);
            Thread thread = new Thread(threadStart);
            thread.Name = "TcpClientConnection Thread";
            thread.Start();
        }

        /// <summary>
        /// Runs the networking thread, dispatching received events to listeners.
        /// </summary>
        private void RunNetworkingThread()
        {
            stream = client.GetStream();
            RunReceiveThread();
        }
    }
}
