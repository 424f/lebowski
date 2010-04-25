using System;
using System.Threading;
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
			serverConnection.DispatchAll();
			clientConnection.DispatchAll();			
			Assert.AreEqual("foo", clientContext.Data);
			Assert.AreEqual("foo", serverContext.Data);			
			
			serverContext.Insert(serverContext, new InsertOperation('!', 0));			
			serverConnection.DispatchAll();
			clientConnection.DispatchAll();						
			Assert.AreEqual("!foo", clientContext.Data);
			Assert.AreEqual("!foo", serverContext.Data);
			
		}
		
		[Test]
		public void TestSelectionPreservation()
		{
			LocalConnection serverConnection = new LocalConnection();
			LocalConnection clientConnection = new LocalConnection();
			LocalProtocol.Connect(serverConnection, clientConnection);
			
			StringTextContext serverContext = new StringTextContext();
			StringTextContext clientContext = new StringTextContext();
			
			var server = new DifferentialSynchronizationStrategy(0, serverContext, serverConnection);
			var client = new DifferentialSynchronizationStrategy(1, clientContext, clientConnection);
			
			clientContext.Insert(clientContext, new InsertOperation("foo", 0));	
			clientContext.SetSelection(0, 3);
			
			// TODO: find better solution for testing than sleeping (e.g. not using
			// a dispatcher thread)
			
			Thread.Sleep(100);
			
			serverContext.Insert(clientContext, new InsertOperation("bar", 0));
			Thread.Sleep(100);
			Assert.AreEqual("foo", clientContext.SelectedText);
			
			serverContext.Insert(clientContext, new InsertOperation("bar", 6));
			Thread.Sleep(100);
			Assert.AreEqual("foo", clientContext.SelectedText);			
		}
	}
}
