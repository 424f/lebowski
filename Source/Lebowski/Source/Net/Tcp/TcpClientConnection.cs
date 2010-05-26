using System;

namespace Lebowski.Net.Tcp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using log4net;
    
    public class TcpClientConnection : TcpConnection
    {
        private static ILog Logger = LogManager.GetLogger(typeof(TcpConnection));

        TcpClient client;

        public TcpClientConnection(string address, int port)
        {
            try
            {
                client = new TcpClient();
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

        protected void RunNetworkingThread()
        {
            stream = client.GetStream();
            RunReceiveThread();
        }
    }
}
