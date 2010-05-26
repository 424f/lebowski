namespace TwinEditor.Sharing
{
    using System;
    using TwinEditor.Execution;
    
    public sealed class StartedExecutionEventArgs : EventArgs
    {
        /// <summary>
        /// The result object dispatches events about this execution
        /// </summary>
        public ExecutionResult ExecutionResult { get; private set; }
        /// <summary>
        /// The id of the site where this execution originated
        /// </summary>
        public int SiteId { get; private set; }

        public StartedExecutionEventArgs(int siteId, ExecutionResult executionResult)
        {
            ExecutionResult = executionResult;
            SiteId = siteId;
        }
    }
}
