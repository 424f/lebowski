namespace TwinEditor.Execution
{
    using System;

    /// <summary>
    /// A helper class that can be passed around and does nothing but 
    /// fire events that it receives from the origin of the execution.
    /// </summary>
    public class ExecutionResult
    {
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

        /// <summary>
        /// Occurs when the execution finished.
        /// </summary>
        public event EventHandler<FinishedExecutionEventArgs> FinishedExecution;
        
        /// <summary>
        /// Occurs when the execution changed its state.
        /// </summary>
        public event EventHandler<ExecutionChangedEventArgs> ExecutionChanged;

    }
}