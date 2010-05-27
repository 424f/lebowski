namespace TwinEditor.UI
{
    using System;
    using Lebowski.Net;
    using TwinEditor.Sharing;

    public sealed class ShareSessionEventArgs : EventArgs
    {
        public ICommunicationProtocol Protocol { get; private set; }
        public SessionContext Session { get; private set; }

        public ShareSessionEventArgs(SessionContext session, ICommunicationProtocol protocol)
        {
            Protocol = protocol;
            Session = session;
        }
    }
}
