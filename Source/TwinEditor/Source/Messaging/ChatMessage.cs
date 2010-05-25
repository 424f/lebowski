namespace TwinEditor.Messaging
{
    using System;    
    
    /// <summary>
    /// Occurs when the user has entered a chat message to be broadcast
    /// to the other users
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
