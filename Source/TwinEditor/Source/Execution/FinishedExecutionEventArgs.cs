namespace TwinEditor.Execution
{
    using System;
    
    public class FinishedExecutionEventArgs : EventArgs
    {
        public string StandardOut { get; private set; }
        public int ReturnCode { get; private set; }
        
        public FinishedExecutionEventArgs(int returnCode, string standardOut)
        {
            ReturnCode = returnCode;
            StandardOut = standardOut;
        }
    }
}
