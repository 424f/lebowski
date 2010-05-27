namespace LebowskiTests.Net.Multichannel
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Lebowski.Net;
    using Lebowski.Net.Local;
    using Lebowski.Net.Multichannel;
    
    [TestFixture]
    public class MultichannelConnectionTest
    {
        const int maxWaitTime = 250;
        
        LocalConnection serverConnection;
        LocalConnection clientConnection;
        
        [SetUp]
        public void SetUp()
        {
            log4net.Config.BasicConfigurator.Configure();
            
            serverConnection = new LocalConnection();
            clientConnection = new LocalConnection();
            LocalProtocol.Connect(serverConnection, clientConnection);        
        }
        
        [TearDown]
        public void TearDown()
        {
            clientConnection.Close();
            serverConnection.Close();
        }
        
        [Test]
        public void TestChannelCreation()
        {
            MultichannelConnection mc1 = new MultichannelConnection(serverConnection);
            var s1 = mc1.CreateChannel();
            var s2 = mc1.CreateChannel();
            
            MultichannelConnection mc2 = new MultichannelConnection(clientConnection);
            var c1 = mc2.CreateChannel();
            var c2 = mc2.CreateChannel();            
        }
        
        [Test]
        public void TestSingleChannel()
        {
            MultichannelConnection mc1 = new MultichannelConnection(serverConnection);
            var s1 = mc1.CreateChannel();
            
            MultichannelConnection mc2 = new MultichannelConnection(clientConnection);
            var c1 = mc2.CreateChannel();
            
            var srecv1 = new List<object>();
            s1.Received += delegate(object sender, ReceivedEventArgs e)
            {
                srecv1.Add(e.Message);
            };
            
            var crecv1 = new List<object>();
            c1.Received += delegate(object sender, ReceivedEventArgs e)
            {
                crecv1.Add(e.Message);
            };   
            
            c1.Send("HI");
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HI"}, srecv1), maxWaitTime);
            
            s1.Send("HI YOURSELF");
            s1.Send("HELLO WORLD");
            c1.Send("OH HAI");
            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HI", "OH HAI"}, srecv1), maxWaitTime);            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HI YOURSELF", "HELLO WORLD"}, crecv1), maxWaitTime);            
        }        
        
        [Test]
        public void TestDualChannel()
        {
            MultichannelConnection mc1 = new MultichannelConnection(serverConnection);
            var s1 = mc1.CreateChannel();
            var s2 = mc1.CreateChannel();
            
            MultichannelConnection mc2 = new MultichannelConnection(clientConnection);
            var c1 = mc2.CreateChannel();
            var c2 = mc2.CreateChannel();
            
            var srecv1 = new List<object>();
            s1.Received += delegate(object sender, ReceivedEventArgs e)
            {
                srecv1.Add(e.Message);
            };

            var srecv2 = new List<object>();
            s2.Received += delegate(object sender, ReceivedEventArgs e)
            {
                srecv2.Add(e.Message);
            };
            
            var crecv1 = new List<object>();
            c1.Received += delegate(object sender, ReceivedEventArgs e)
            {
                crecv1.Add(e.Message);
            };   

            var crecv2 = new List<object>();
            c2.Received += delegate(object sender, ReceivedEventArgs e)
            {
                crecv2.Add(e.Message);
            };               
            
            c1.Send("HI");
            s2.Send("HI YOURSELF");
            c2.Send("HELLO WORLD");
            s1.Send("OH HAI");
            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HI"}, srecv1), maxWaitTime);            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HELLO WORLD"}, srecv2), maxWaitTime);            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"OH HAI"}, crecv1), maxWaitTime);            
            TestUtil.WaitUntil(() => CollectionAssert.AreEqual(new object[]{"HI YOURSELF"}, crecv2), maxWaitTime);            
        }               
        
        [Test]
        public void TestTooManyChannels()
        {
            MultichannelConnection mc1 = new MultichannelConnection(serverConnection);
            var s1 = mc1.CreateChannel();
            
            MultichannelConnection mc2 = new MultichannelConnection(clientConnection);
            var c1 = mc2.CreateChannel();
            var c2 = mc2.CreateChannel();            
            
            var srecv1 = new List<object>();
            s1.Received += delegate(object sender, ReceivedEventArgs e)
            {
                srecv1.Add(e.Message);
            };           
            
            c2.Send("THIS SHOULD BE IGNORED");
        }
    }
}
