
using System;

namespace TwinEditor.Messaging
{
    /// <summary>
    /// Sent when a shared program's execution yields additional results
    /// (e.g. compilation error, stdout, completed execution)
    /// </summary>
    [Serializable]
    public sealed class ExecutionResultMessage
    {
        public string StandardOut { get; private set; }
        public bool Finished { get; private set; }
        
        public ExecutionResultMessage()
        {
        }
    }
}
