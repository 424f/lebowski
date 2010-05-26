namespace TwinEditor.Messaging
{
    using System;

    /// <summary>
    /// Sent when a shared program's execution finishes.
    /// </summary>
    [Serializable]
    public sealed class ExecutionResultMessage
    {
        /// <summary>
        /// Initializes a new instance of the ExecutionResultMessage, given
        /// the execution's returnCode and output.
        /// </summary>
        /// <param name="returnCode">See <see cref="ReturnCode">ReturnCode</see>.</param>
        /// <param name="standardOut">See <see cref="StandardOut">StandardOut</see>.</param>
        public ExecutionResultMessage(int returnCode, string standardOut)
        {
            StandardOut = standardOut;
            ReturnCode = returnCode;
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