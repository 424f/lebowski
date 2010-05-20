using System;
using Lebowski.Net;
using Lebowski.Net.Tcp;
using NUnit.Framework;

namespace LebowskiTests.Net.Tcp
{
	[TestFixture]
	public class TcpConnectionTest
	{
		private int port = 12345;
		private TcpServerConnection server = null;
		
		public TcpConnectionTest()
		{
		}
		
		[SetUp]
		public void SetUp()
		{
			server = new TcpServerConnection(port);
		}
		
		[TearDown]
		public void TearDown()
		{
			server.Close();
			server = null;
		}
		
		
		[Test]
		public void TestHostAndConnect()
		{
			string serverReceived = "";
			string clientReceived = "";
			
			TcpClientConnection client = new TcpClientConnection("localhost", port);
			
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
			
			server.Close();
		}
		
		[Test]
		/// <summary>
		/// At the moment, we do not support multiple connections, so after an initial
		/// client connection, every connection attempt after that should fail. The initial
		/// connection should still work after that.
		/// </summary>
		public void TestDiscardMultipleConnections()
		{
			TcpClientConnection client = new TcpClientConnection("localhost", port);
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
			TcpClientConnection client = new TcpClientConnection("localhost", port);

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
			
			TestUtil.WaitAreEqual(true, () => connectionClosed, 250);
			
		}		
	}
}
