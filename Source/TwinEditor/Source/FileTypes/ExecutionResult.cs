using System;

namespace TwinEditor
{
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
    
    public class ExecutionChangedEventArgs : EventArgs
    {
        public string StandardOut { get; private set; }
        
        public ExecutionChangedEventArgs(string standardOut)
        {
            StandardOut = standardOut;
        }
    }
    
    public class ExecutionResult
    {
        public event EventHandler<FinishedExecutionEventArgs> FinishedExecution;
        public event EventHandler<ExecutionChangedEventArgs> ExecutionChanged;
        
        internal virtual void OnFinishedExecution(FinishedExecutionEventArgs e)
        {
            if (FinishedExecution != null)
            {
                FinishedExecution(this, e);
            }
        }
        
        internal virtual void OnExecutionChanged(ExecutionChangedEventArgs e)
        {
            if (ExecutionChanged != null)
            {
                ExecutionChanged(this, e);
            }
        }
        
        
    }
}
