namespace TwinEditor.Messaging
{
    using System;

    /// <summary>
    /// Sent when the a chat message is broadcast to other sites.
    /// </summary>
    [Serializable]
    public sealed class ChatMessage
    {
        public string UserName { get; private set; }
        public string Message { get; private set; }

        public ChatMessage(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }
    }
}