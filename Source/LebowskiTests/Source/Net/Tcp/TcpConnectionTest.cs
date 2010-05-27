
namespace LebowskiTests.Net.Tcp
{
    using System;
    using Lebowski.Net;
    using Lebowski.Net.Tcp;
    using NUnit.Framework;
    [TestFixture]
    public class TcpConnectionTest
    {
        private int port = 12345;
        private TcpServerConnection server = null;
        private TcpClientConnection client = null;

        public TcpConnectionTest()
        {
        }

        [SetUp]
        public void SetUp()
        {
            server = new TcpServerConnection(port);
            client = new TcpClientConnection("localhost", port);            
        }

        [TearDown]
        public void TearDown()
        {
            if(server != null)
            {
                server.Close();
                server = null;
            }
            
            if(client != null)
            {
                client.Close();
                client = null;
            }
        }


        [Test]
        public void TestHostAndConnect()
        {
        }
        
        [Test]
        public void TestSimpleSession()
        {
            string serverReceived = "";
            string clientReceived = "";

            server.Received += delegate(object sender, ReceivedEventArgs e)
            {
                serverReceived += e.Message.ToString();
            };

            client.Received += delegate(object sender, ReceivedEventArgs e)
            {
                clientReceived += e.Message.ToString();
            };

            client.Send("Foo");
            server.Send("Bar");
            client.Send("Bar");
            server.Send("Foo");

            TestUtil.WaitAreEqual("FooBar", () =>  serverReceived, 500);
            TestUtil.WaitAreEqual("BarFoo", () =>  clientReceived, 500);
        }

        [Test]
        public void TestDelayedRegistration()
        {
            string serverReceived = "";
            string clientReceived = "";

            client.Received += delegate(object sender, ReceivedEventArgs e)
            {
                clientReceived += e.Message.ToString();
            };

            client.Send("Foo");            
            server.Send("Bar");
            client.Send("Bar");
            server.Send("Foo");
            
            // We just now register the event handler, but the previously
            // received messages should have been queued up and dispatched now.
            server.Received += delegate(object sender, ReceivedEventArgs e)
            {
                serverReceived += e.Message.ToString();
            };                        

            TestUtil.WaitAreEqual("FooBar", () =>  serverReceived, 500);
            TestUtil.WaitAreEqual("BarFoo", () =>  clientReceived, 500);

            server.Close();
            client.Close();
        }        
        
        [Test]
        /// <summary>
        /// At the moment, we do not support multiple connections, so after an initial
        /// client connection, every connection attempt after that should fail. The initial
        /// connection should still work after that.
        /// </summary>
        public void TestDiscardMultipleConnections()
        {
            Assert.Throws(typeof(ConnectionFailedException), delegate { var c = new TcpClientConnection("localhost", port); });
            Assert.Throws(typeof(ConnectionFailedException), delegate { var c = new TcpClientConnection("localhost", port); });
            Assert.Throws(typeof(ConnectionFailedException), delegate { var c = new TcpClientConnection("localhost", port); });

            string serverReceived = "";
            server.Received += delegate(object sender, ReceivedEventArgs e)
            {
                serverReceived += e.Message.ToString();
            };

            string text = "Good day to you, sir";

            client.Send(text);

            TestUtil.WaitAreEqual(text, () => serverReceived, 250);

            server.Close();
        }

        [Test]
        /// <summary>
        /// Tests behavior when server shuts down regularly after establishing a connection
        /// </summary>
        public void TestServerShutdown()
        {
            string serverReceived = "";
            bool connectionClosed = false;

            server.Received += delegate(object sender, ReceivedEventArgs e)
            {
                serverReceived += e.Message.ToString();
            };

            client.ConnectionClosed += delegate(object sender, EventArgs e)
            {
                connectionClosed = true;
            };

            string text = "Good day to you, sir";

            client.Send(text);

            TestUtil.WaitAreEqual(text, () => serverReceived, 250);
            server.Close();
            server = null;

            TestUtil.WaitAreEqual(true, () => connectionClosed, 750);

        }
    }
}