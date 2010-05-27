namespace LebowskiTests.Synchronization
{
    using System;
    using System.Threading;
    using Lebowski;
    using Lebowski.TextModel;
    using Lebowski.TextModel.Operations;
    using Lebowski.Net;
    using Lebowski.Net.Local;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using NUnit.Framework;

    [TestFixture]
    public class DifferentialSynchronizationTest
    {
        const int maxWaitTime = 250;
        
        LocalConnection serverConnection;
        LocalConnection clientConnection;
        
        StringTextContext serverContext;
        StringTextContext clientContext;
        
        ISynchronizationStrategy server;
        ISynchronizationStrategy client;
        
        [SetUp]
        public void SetUp()
        {
            log4net.Config.BasicConfigurator.Configure();
            
            serverConnection = new LocalConnection();
            clientConnection = new LocalConnection();
            LocalProtocol.Connect(serverConnection, clientConnection);

            serverContext = new StringTextContext();
            clientContext = new StringTextContext();

            server = new DifferentialSynchronizationStrategy(0, serverContext, serverConnection);
            client = new DifferentialSynchronizationStrategy(1, clientContext, clientConnection);            
        }

        [Test]
        public void TestSimpleSession()
        {
            clientContext.Insert(clientContext, new InsertOperation('f', 0));
            clientContext.Insert(clientContext, new InsertOperation('o', 1));
            clientContext.Insert(clientContext, new InsertOperation('o', 2));

            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            serverContext.Insert(serverContext, new InsertOperation('!', 0));
            serverConnection.DispatchAll();
            clientConnection.DispatchAll();
            TestUtil.WaitAreEqual("!foo", () => serverContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("!foo", () => clientContext.Data, maxWaitTime);
        }

        [Test]
        public void TestDeletion()
        {
            clientContext.Insert(clientContext, new InsertOperation('f', 0));
            clientContext.Insert(clientContext, new InsertOperation('o', 1));
            clientContext.Insert(clientContext, new InsertOperation('o', 2));

            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            serverContext.SetDataAsUser("");
            TestUtil.WaitAreEqual("", () => serverContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("", () => clientContext.Data, maxWaitTime);
        }        
        
        [Test]
        public void TestDeletion2()
        {
            clientContext.Insert(clientContext, new InsertOperation('f', 0));
            clientContext.Insert(clientContext, new InsertOperation('o', 1));
            clientContext.Insert(clientContext, new InsertOperation('o', 2));

            clientContext.CaretPosition = 2;
            clientContext.SetSelectionAsUser(2, 3);
            
            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            serverContext.SetDataAsUser("oo");
            TestUtil.WaitAreEqual("oo", () => serverContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("oo", () => clientContext.Data, maxWaitTime);
            Assert.AreEqual(1, clientContext.CaretPosition);
            Assert.AreEqual(1, clientContext.SelectionStart);
            Assert.AreEqual(2, clientContext.SelectionEnd);
        }             
        
        [Test]
        public void TestAsynchronousSession()
        {
            clientContext.SetDataAsUser("bbb");

            TestUtil.WaitAreEqual("bbb", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("bbb", () => serverContext.Data, maxWaitTime);
            
            // Now both users edit asynchronously
            clientConnection.PauseSending();
            serverConnection.PauseSending();

            clientContext.SetDataAsUser("aaa bbb");
            serverContext.SetDataAsUser("bbb ccc");
            
            // And the connection is up again
            clientConnection.ResumeSending();
            serverConnection.ResumeSending();
            
            TestUtil.WaitAreEqual("aaa bbb ccc", () => serverContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("aaa bbb ccc", () => clientContext.Data, maxWaitTime);
        }        
        
        [Test]
        public void TestSelectionPreservation()
        {
            clientContext.Insert(clientContext, new InsertOperation("foo", 0));
            clientContext.CaretPosition = 2;
            clientContext.SetSelectionAsUser(0, 3);
            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            serverContext.Insert(serverContext, new InsertOperation("bar", 0));
            TestUtil.WaitAreEqual("barfoo", () => clientContext.Data, maxWaitTime);
            Assert.AreEqual("foo", clientContext.SelectedText);
            Assert.AreEqual(5, clientContext.CaretPosition);

            serverContext.Insert(serverContext, new InsertOperation("bar", 6));
            TestUtil.WaitAreEqual("barfoobar", () => clientContext.Data, maxWaitTime);
            Assert.AreEqual("foo", clientContext.SelectedText);
            Assert.AreEqual(5, clientContext.CaretPosition);
        }
        
        [Test]
        public void TestSelectionPreservation2()
        {
            clientContext.Insert(clientContext, new InsertOperation("foo", 0));
            clientContext.CaretPosition = 2;
            clientContext.SetSelectionAsUser(0, 3);
            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            serverContext.Insert(serverContext, new InsertOperation("rr", 1));
            TestUtil.WaitAreEqual("frroo", () => clientContext.Data, maxWaitTime);
            Assert.AreEqual("frroo", clientContext.SelectedText);
            Assert.AreEqual(4, clientContext.CaretPosition);
        }        
        
        [Test]
        public void TestRemoteSelection()
        {
            clientContext.Insert(clientContext, new InsertOperation("foo", 0));
            TestUtil.WaitAreEqual("foo", () => clientContext.Data, maxWaitTime);
            TestUtil.WaitAreEqual("foo", () => serverContext.Data, maxWaitTime);

            clientContext.SetSelectionAsUser(0, 3);
            
            TestUtil.WaitAreEqual(true, () => serverContext.RemoteSelectionStart == 0 && serverContext.RemoteSelectionEnd == 3, maxWaitTime);
            
        }           
    }
}