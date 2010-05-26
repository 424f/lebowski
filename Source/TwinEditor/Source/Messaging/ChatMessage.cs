namespace TwinEditor.Messaging
{
    using System;

    /// <summary>
    /// Sent when the a chat message is broadcast to other sites.
    /// </summary>
    [Serializable]
    public sealed class ChatMessage
    {
        /// <summary>
        /// Initializes a new instance of the ChatMessage class,
        /// given a message and the issuer's user name.
        /// </summary>
        /// <param name="userName">See <see cref="UserName">UserName</see>.</param>
        /// <param name="message">See <see cref="Message">Message</see>.</param>
        public ChatMessage(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }

        /// <summary>
        /// The name of the user who sent this message.
        /// </summary>
        public string UserName { get; private set; }
        
        /// <summary>
        /// The message's text.
        /// </summary>
        public string Message { get; private set; }        
    }
}