namespace TwinEditor.Sharing
{
    using System;
    using TwinEditor.Execution;
    
    /// <summary>
    /// Provides data for the <see cref="SessionContext.StartedExecution">SessionContext.StartedExecution</see> event.
    /// </summary>
    public sealed class StartedExecutionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the StartedExecutionEventArgs class.
        /// </summary>
        /// <param name="siteId">See <see cref="SiteId">SiteId</see>.</param>
        /// <param name="executionResult">See <see cref="ExecutionResult">ExecutionResult</see>.</param>
        public StartedExecutionEventArgs(int siteId, ExecutionResult executionResult)
        {
            ExecutionResult = executionResult;
            SiteId = siteId;
        }        
        
        /// <summary>
        /// The result object dispatches events about this execution
        /// </summary>
        public ExecutionResult ExecutionResult { get; private set; }
        
        /// <summary>
        /// The id of the site where this execution originated
        /// </summary>
        public int SiteId { get; private set; }
    }
}
