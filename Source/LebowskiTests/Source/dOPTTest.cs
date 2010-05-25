namespace LebowskiTests
{
    using System;
    using NUnit.Framework;
    using Lebowski.Synchronization.dOPT;
    using Lebowski.TextModel;
    using Lebowski.TextModel.Operations;
    using ModelType = Lebowski.Synchronization.dOPT.Model<Lebowski.TextModel.Operations.TextOperation, Lebowski.TextModel.ITextContext>;    
    
    /// <summary>
    /// Tests the dOPT implementation using text operaitons. Note that some tests
    /// expectedly fail, as dOPT cannot guarantee convergence in all cases.
    /// </summary>
    [TestFixture]
    public class dOPTTest
    {
        public dOPTTest()
        {
        }
        
        [Test]
        public void testSimple()
        {
            StringTextContext c1 = new StringTextContext();
            StringTextContext c2 = new StringTextContext();
            
            ModelType server = new ModelType(0, c1);
            ModelType client = new ModelType(1, c2);
            
            server.Generate(new InsertOperation('w', 0));
            server.Generate(new InsertOperation('h', 1));
            server.Generate(new InsertOperation('a', 2));
            server.Generate(new InsertOperation('t', 3));
            server.DeliverBroadcasts(new ModelType[]{ client });
            client.DeliverBroadcasts(new ModelType[]{ server });
            
            Assert.AreEqual("what", client.Context.Data);
            Assert.AreEqual("what", server.Context.Data);
        }
        
        [Test]
        public void testTwoWay()
        {
            StringTextContext c1 = new StringTextContext();
            StringTextContext c2 = new StringTextContext();
            
            ModelType server = new ModelType(0, c1);
            ModelType client = new ModelType(1, c2);
            
            server.Generate(new InsertOperation('d', 0));
            server.Generate(new InsertOperation('e', 1));
            server.Generate(new InsertOperation('f', 2));
            server.DeliverBroadcasts(new ModelType[]{ client });
            client.DeliverBroadcasts(new ModelType[]{ server });
            Assert.AreEqual("def", client.Context.Data);
            Assert.AreEqual("def", server.Context.Data);            
            
            server.Generate(new DeleteOperation(0));
            client.Generate(new InsertOperation('f', 3));
            client.Generate(new InsertOperation('e', 4));
            client.Generate(new InsertOperation('c', 5));
            client.Generate(new InsertOperation('t', 6));
            server.DeliverBroadcasts(new ModelType[]{ client });
            client.DeliverBroadcasts(new ModelType[]{ server });            
            
            Assert.AreEqual("effect", client.Context.Data);
            Assert.AreEqual("effect", server.Context.Data);
        }        
    }
}
