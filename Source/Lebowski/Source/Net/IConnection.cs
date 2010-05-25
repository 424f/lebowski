
using System;

namespace Lebowski.Net
{
    public interface IConnection
    {
        void Send(object o);
        void Close();
        
        event EventHandler<ReceivedEventArgs> Received;
        
        /// <summary>
        /// Can be used to associate additional data with this connection
        /// </summary>
        object Tag { get; set; }
    }
    
    public class ReceivedEventArgs : EventArgs
    {
        public object Message { get; protected set; }
        
        public ReceivedEventArgs(object message)
        {
            Message = message;
        }
    }    
}
