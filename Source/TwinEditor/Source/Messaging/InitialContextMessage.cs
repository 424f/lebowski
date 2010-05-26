
using System;

namespace TwinEditor.Messaging
{
    /// <summary>
    /// Sent to initially synchronize the client's context with the server's
    /// </summary>
    [Serializable]
    public class InitialContextMessage
    {
        /// <summary>
        /// Initializes a new instance of the InitialContextMessage class with
        /// the data of the context.
        /// <param name="text">The server's context data.</param>
        /// </summary>
        public InitialContextMessage(string text)
        {
            Text = text;
        }
        
        /// <summary>
        /// The server's context data.
        /// </summary>
        public string Text { get; private set; }
    }
}
