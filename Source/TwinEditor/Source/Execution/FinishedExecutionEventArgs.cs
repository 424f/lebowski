namespace TwinEditor.Execution
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="ExecutionResult.FinishedExecution">ExecutionResult.FinishedExecution</see> event.
    /// </summary>    
    public class FinishedExecutionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FinishedExecutionEventArgs class.
        /// </summary>
        /// <param name="returnCode">See <see cref="ReturnCode">ReturnCode</see>.</param>
        /// <param name="standardOut">See <see cref="StandardOut">StandardOut</see>.</param>
        public FinishedExecutionEventArgs(int returnCode, string standardOut)
        {
            ReturnCode = returnCode;
            StandardOut = standardOut;
        }
        
        /// <summary>
        /// The output to the StandardOut that the program produced
        /// during its execution.
        /// </summary>
        public string StandardOut { get; private set; }
        
        /// <summary>
        /// The return code the execution yielded.
        /// </summary>        
        public int ReturnCode { get; private set; }
        
    }
}