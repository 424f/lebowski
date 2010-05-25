namespace TwinEditor.Execution
{   
    using System;
    
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
