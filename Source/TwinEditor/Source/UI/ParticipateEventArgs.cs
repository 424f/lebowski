namespace TwinEditor.UI
{
    using System;
    using Lebowski.Net;
    using TwinEditor.Sharing;

    /// <summary>
    /// Provides data for the <see cref="IApplicationView.Participate" /> event.
    /// </summary>
    public sealed class ParticipateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ParticipateEventArgs with a session
        /// to share and the protocol to share it with.
        /// </summary>
        /// <param name="session">See <see cref="Session" />.</param>
        /// <param name="protocol">See <see cref="Protocol" />.</param>
        public ParticipateEventArgs(SessionContext session, ICommunicationProtocol protocol)
        {
            Protocol = protocol;
            Session = session;
        }
        
        /// <summary>
        /// The protocol that the session should be shared with.
        /// </summary>
        public ICommunicationProtocol Protocol { get; private set; }
        
        /// <summary>
        /// The session to be shared.
        /// </summary>
        public SessionContext Session { get; private set; }
        
    }
}
