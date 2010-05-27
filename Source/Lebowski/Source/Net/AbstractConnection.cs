namespace Lebowski.Net
{
    using System;
    using System.Threading;
    using log4net;
    
    /// <summary>
    /// Provides functionality that classes implementing <see cref="Lebowski.Net.IConnection">IConnection</see>
    /// usually share.
    /// </summary>
    public abstract class AbstractConnection : IConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AbstractConnection));    

        /// <inheritdoc/>
        public object Tag { get; set; }        

        /// <inheritdoc/>
        public abstract void Send(object o);        
        
        /// <inheritdoc/>
        public abstract void Close();        
        
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
        /// Raises the Received event.
        /// </summary>
        /// <param name="e">A ReceivedEventArgs that contains the event data.</param>
        protected virtual void OnReceived(ReceivedEventArgs e)
        {
            /* As we should not dispatch packets when nobody is listening,
             * we wait until there is at least one listener */
            if(Received == null || Received.GetInvocationList().Length == 0)
            {
                Logger.InfoFormat("No listener attached to {0}.Received, waiting with dispatching.", this);
                while(Received == null || Received.GetInvocationList().Length == 0)
                {
                    // TODO: solve this using activation / deactivation
                    Thread.Sleep(100);
                }            
            }
            if (Received != null)
            {
                try
                {
                    Received(this, e);
                }
                catch(Exception exception)
                {
                    Logger.ErrorFormat("En exception occurred when dispatching {0}:\n{1}", e, exception);
                }
            }
        }      
        
        /// <inheritdoc/>
        public event EventHandler<ReceivedEventArgs> Received; 
        
        /// <inheritdoc/>
        public event EventHandler<EventArgs> ConnectionClosed;        
               
    }
}
