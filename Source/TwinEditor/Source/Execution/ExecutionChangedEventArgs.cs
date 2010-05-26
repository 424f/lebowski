namespace TwinEditor.Execution
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="ExecutionResult.ExecutionChanged">ExecutionResult.ExecutionChanged</see> event.
    /// </summary>
    public class ExecutionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ExecutionChangedEventArgs with
        /// the standard output from an execution.
        /// </summary>
        /// <param name="standardOut">See <see cref="StandardOut">StandardOut</see>.</param>
        public ExecutionChangedEventArgs(string standardOut)
        {
            StandardOut = standardOut;
        }
        
        /// <summary>
        /// The standard output produced by an execution.
        /// </summary>
        public string StandardOut { get; private set; }        
    }
}