namespace LebowskiTests.Net
{
    using System;
    using Lebowski.Net;
    using NUnit.Framework;
    using System.Runtime.Serialization;

    [Serializable]
    class TestMessage
    {
        internal TestMessage(string message)
        {
            Message = message;
        }
        
        internal string Message;
    }
    
    class NonSerialiable {}
    
    [TestFixture]
    public class NetUtilsTest
    {
        [Test]
        public void TestSerialization()
        {
            byte[] data = NetUtils.Serialize(new TestMessage("GOOD DAY TO YOU SIR"));
        }

        [Test]
        public void TestSerializationAndDeserialization()
        {
            string text = "GOOD DAY TO YOU SIR";
            byte[] data = NetUtils.Serialize(new TestMessage(text));
            object obj = NetUtils.Deserialize(data);
            Assert.IsInstanceOf(typeof(TestMessage), obj);
            TestMessage testMessage = (TestMessage)obj;
            Assert.AreEqual(text, testMessage.Message);
        }        
        
        [Test]
        public void TestNonSerializableType()
        {
            Assert.Throws(typeof(SerializationException), () => NetUtils.Serialize(new NonSerialiable()));
        }
    }
}
