namespace Lebowski.Net
{
    using System;
    using System.Threading;
    
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
            /* As we should not dispatch packets when nobody is listening,
             * we wait until there is at least one listener */
            while(Received.GetInvocationList().Length == 0)
            {
                // TODO: solve this using activation / decativation
                Thread.Sleep(100);
            }            
            if (Received != null)
            {
                Received(this, e);
            }
        }        
        
        
    }
}
