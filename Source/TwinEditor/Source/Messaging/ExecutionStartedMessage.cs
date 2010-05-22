
using System;

namespace TwinEditor.Messaging
{
    /// <summary>
    /// Sent when a user executes a program which is shared in a
    /// collaborative editing session
    /// </summary>
    [Serializable]
    public sealed class ExecutionStartedMessage
    {
        public string Arguments { get; private set; }
        
        public ExecutionStartedMessage()
        {
            
        }
    }
}
