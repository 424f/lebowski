using System;

namespace Lebowski.Net
{
    /// <summary>
    /// Provides data for the <see cref="Lebowski.Net.ICommunicationProtocol.JoinSession">JoinSession</see> event.
    /// </summary>
    public sealed class JoinSessionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the HostSessionEventArgs class.
        /// </summary>
        /// <param name="connection">See <see cref="Lebowski.Net.JoinSessionEventArgs.Connection">Connection</see></param>        
        public JoinSessionEventArgs(IConnection connection)
        {
            Connection = connection;
        }
        
        /// <summary>
        /// Gets the connection that is going to be used for this session.
        /// </summary>        
        public IConnection Connection { get; private set; }        
    }
}
