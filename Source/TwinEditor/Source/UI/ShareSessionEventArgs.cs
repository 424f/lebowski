namespace TwinEditor.UI
{
    using System;
    using Lebowski.Net;
    using TwinEditor.Sharing;

    /// <summary>
    /// Provides data for the <see cref="IApplicationView.ShareSession" /> event.
    /// </summary>
    public sealed class ShareSessionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ShareSessionEventArgs with the session
        /// to be shared and the communication protocol to be used.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="protocol"></param>
        public ShareSessionEventArgs(SessionContext session, ICommunicationProtocol protocol)
        {
            Protocol = protocol;
            Session = session;
        }
        
        /// <summary>
        /// The communication protocol to be used to share the session.
        /// </summary>
        public ICommunicationProtocol Protocol { get; private set; }
        
        /// <summary>
        /// The session to be used.
        /// </summary>
        public SessionContext Session { get; private set; }        
    }
}
