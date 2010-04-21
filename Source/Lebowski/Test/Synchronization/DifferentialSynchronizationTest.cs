using System;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using NUnit.Framework;

namespace Lebowski.Test.Synchronization
{
	[TestFixture]
	public class DifferentialSynchronizationTest
	{
		public DifferentialSynchronizationTest()
		{
		}

		[Test]
		public void TestThis()
		{
			LocalConnection serverConnection = new LocalConnection();
			LocalConnection clientConnection = new LocalConnection();
			LocalProtocol.Connect(serverConnection, clientConnection);
			
			ITextContext serverContext = new StringTextContext();
			ITextContext clientContext = new StringTextContext();
			
			var server = new DifferentialSynchronizationStrategy(0, serverContext, serverConnection);
			var client = new DifferentialSynchronizationStrategy(1, clientContext, clientConnection);
			
			clientContext.Insert(clientContext, new InsertOperation('f', 0));
			clientContext.Insert(clientContext, new InsertOperation('o', 1));
			clientContext.Insert(clientContext, new InsertOperation('o', 2));
			
			Assert.AreNotEqual(client.State, server.State);
			
			server.FlushToken();
			
			Assert.AreNotEqual(client.State, server.State);
			
			serverContext.Insert(serverContext, new InsertOperation('!', 0));
			
			Assert.AreNotEqual(client.State, server.State);
			
			client.FlushToken();
			server.FlushToken();
			client.FlushToken();
			
			Assert.AreNotEqual(client.State, server.State);
			
			Assert.AreEqual(clientContext.Data, serverContext.Data);

			Assert.AreEqual("!foo", clientContext.Data);
			Assert.AreEqual("!foo", serverContext.Data);
			
		}
	}
}
