namespace Lebowski.Net
{
    using System;
    
    /// <summary>
    /// Provides data for the <see cref="Lebowski.Net.IConnection.Received">Received</see> event.
    /// </summary>    
    public class ReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ReceivedEventArgs class.
        /// </summary>
        /// <param name="message">See <see cref="Lebowski.Net.ReceivedEventArgs.Message">Message</see>.</param>
        public ReceivedEventArgs(object message)
        {
            Message = message;
        }
        
        /// <summary>
        /// The message object that has been received.
        /// </summary>
        public object Message { get; protected set; }        
    }
}
