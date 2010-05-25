namespace Lebowski.Net
{
    using System;
    
    public class ReceivedEventArgs : EventArgs
    {
        public object Message { get; protected set; }

        public ReceivedEventArgs(object message)
        {
            Message = message;
        }
    }
}
