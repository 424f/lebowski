using System;
using System.Threading;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using NUnit.Framework;

using LebowskiTests;

namespace LebowskiTests.Synchronization
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
			TestUtil.WaitAreEqual("foo", () => clientContext.Data, 250);
			TestUtil.WaitAreEqual("foo", () => serverContext.Data, 250);
			
			serverContext.Insert(serverContext, new InsertOperation('!', 0));			
			serverConnection.DispatchAll();
			clientConnection.DispatchAll();
			TestUtil.WaitAreEqual("!foo", () => clientContext.Data, 250);
			TestUtil.WaitAreEqual("!foo", () => serverContext.Data, 250);			
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
			clientContext.CaretPosition = 2;
			clientContext.SetSelection(0, 3);
			
			// TODO: find better solution for testing than sleeping (e.g. not using
			// a dispatcher thread)
			
			Thread.Sleep(100);
			
			serverContext.Insert(clientContext, new InsertOperation("bar", 0));
			Thread.Sleep(100);
			Assert.AreEqual("foo", clientContext.SelectedText);
			Assert.AreEqual(5, clientContext.CaretPosition);
			
			serverContext.Insert(clientContext, new InsertOperation("bar", 6));
			Thread.Sleep(100);
			Assert.AreEqual("foo", clientContext.SelectedText);			
			Assert.AreEqual(5, clientContext.CaretPosition);
		}
		
		[Test]
		/// <summary>
		/// Tests that the client starts out with the server's current context
		/// </summary>
		public void TestSharedContextEstablishment()
		{
            LocalConnection serverConnection = new LocalConnection();
			LocalConnection clientConnection = new LocalConnection();
			LocalProtocol.Connect(serverConnection, clientConnection);
			
			string testString = "foo bar";
			StringTextContext serverContext = new StringTextContext();
			serverContext.Data = testString;
			
			StringTextContext clientContext = new StringTextContext();
			
			var server = new DifferentialSynchronizationStrategy(0, serverContext, serverConnection);
			var client = new DifferentialSynchronizationStrategy(1, clientContext, clientConnection);
			
			server.EstablishSession();
			
			TestUtil.WaitAreEqual(testString, () => clientContext.Data, 250);
		}
	}
}
