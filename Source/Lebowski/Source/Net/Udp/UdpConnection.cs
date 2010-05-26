using Lidgren.Network;

namespace Lebowski.Net.Udp
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using log4net;
    using Lebowski.Net;

    public abstract class UdpConnection : AbstractConnection
    {
        public const string AppName = "LEBOWSKI";
        public const int Port = 12345;

        protected void SerializeToBuffer(object o, NetBuffer buffer)
        {
            buffer.Write(NetUtils.Serialize(o));
        }

        protected object DeserializeFromBuffer(NetBuffer buffer)
        {
            return NetUtils.Deserialize(buffer.ReadBytes(buffer.LengthBytes));
        }

        public override abstract void Send(object o);

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}