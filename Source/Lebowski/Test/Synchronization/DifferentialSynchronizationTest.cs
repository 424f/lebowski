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
			LocalTextConnection serverConnection = new LocalTextConnection();
			LocalTextConnection clientConnection = new LocalTextConnection();
			LocalTextProtocol.Connect(serverConnection, clientConnection);
			
			ITextContext serverContext = new StringTextContext();
			ITextContext clientContext = new StringTextContext();
			
			var server = new DifferentialSynchronizationStrategy(serverContext, serverConnection);
			var client = new DifferentialSynchronizationStrategy(clientContext, clientConnection);
			
			serverContext.Insert(new InsertOperation('f', 0), false);
			serverContext.Insert(new InsertOperation('o', 1), false);
			serverContext.Insert(new InsertOperation('o', 2), false);
			
			System.Threading.Thread.Sleep(100);
			
			Assert.AreEqual("foo", clientContext.Data);
			Assert.AreEqual("foo", serverContext.Data);
		}
	}
}
