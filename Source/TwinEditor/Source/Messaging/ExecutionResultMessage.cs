namespace TwinEditor.Messaging
{
    using System;
    
    /// <summary>
    /// Sent when a shared program's execution finishes
    /// </summary>
    [Serializable]
    public sealed class ExecutionResultMessage
    {
        public string StandardOut { get; private set; }
        public int ReturnCode { get; private set; }
        
        public ExecutionResultMessage(int returnCode, string standardOut)
        {
            StandardOut = standardOut;
            ReturnCode = returnCode;
        }
    }
}
