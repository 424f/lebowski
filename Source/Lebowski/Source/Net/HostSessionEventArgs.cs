namespace Lebowski.Net
{
    using System;
    
    /// <summary>
    /// Provides data for the <see cref="Lebowski.Net.ICommunicationProtocol.HostSession">HostSession</see> event.
    /// </summary>
    public sealed class HostSessionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the HostSessionEventArgs class.
        /// </summary>
        /// <param name="session">See <see cref="Lebowski.Net.HostSessionEventArgs.Session">Session</see></param>
        /// <param name="connection">See <see cref="Lebowski.Net.HostSessionEventArgs.Connection">Connection</see></param>
        public HostSessionEventArgs(ISynchronizationSession session, IConnection connection)
        {
            Session = session;
            Connection = connection;
        }        
        
        /// <summary>
        /// Gets the connection that is going to be used for this session.
        /// </summary>
        public IConnection Connection { get; private set; }
        
        /// <summary>
        /// Gives the synchronization session that is about to be shared.
        /// </summary>
        public ISynchronizationSession Session { get; private set; }
    }
}
