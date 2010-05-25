using System;

namespace Lebowski.Net
{
    /// <summary>
    /// Provides functionality that classes implementing <see cref="Lebowski.Net.IConnection">IConnection</see>
    /// usually share.
    /// </summary>
    public abstract class AbstractConnection : IConnection
    {
        /// <inheritdoc/>
        public event EventHandler<ReceivedEventArgs> Received; 
        
        /// <inheritdoc/>
        public event EventHandler<EventArgs> ConnectionClosed;

        /// <inheritdoc/>
        public abstract void Send(object o);        
        
        /// <inheritdoc/>
        public abstract void Close();
        
        /// <inheritdoc/>
        public object Tag { get; set; }
        
        
        /// <summary>
        /// Raises the ConnectionClosed event
        /// </summary>
        /// <param name="e">A EventArgs.</param>
        protected virtual void OnConnectionClosed(EventArgs e)
        {
            if (ConnectionClosed != null) 
            {
                ConnectionClosed(this, e);
            }
        }

        /// <summary>
        /// Raises the Received event
        /// </summary>
        /// <param name="e">A ReceivedEventArgs that contains the event data.</param>
        protected virtual void OnReceived(ReceivedEventArgs e)
        {
            if (Received != null)
            {
                Received(this, e);
            }
        }        
        
        
    }
}
