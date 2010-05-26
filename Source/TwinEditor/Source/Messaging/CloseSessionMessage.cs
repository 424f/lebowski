namespace TwinEditor.Messaging
{
    using System;

    /// <summary>
    /// Sent when a <see cref="TwinEditor.Sharing.SessionContext">SessionContext</see>
    /// is closed or the session is closed for any other reason.
    /// </summary>
    [Serializable]
    public sealed class CloseSessionMessage
    {
        /// <summary>
        /// Initializes a new instance of the CloseSessionMessage class.
        /// </summary>
        public CloseSessionMessage()
        {
        }
    }
}