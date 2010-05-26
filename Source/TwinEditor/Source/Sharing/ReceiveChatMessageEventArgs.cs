namespace TwinEditor.Sharing
{
    using System;
    using TwinEditor.Messaging;
    
    /// <summary>
    /// Provides data for the <see cref="SessionContext.ReceiveChatMessage">ReceiveChatMessage</see> event.
    /// </summary>
    public sealed class ReceiveChatMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ReceiveChatMessageEventArgs class.
        /// </summary>
        /// <param name="chatMessage">See <see cref="ChatMessage">ChatMessage</see>.</param>
        public ReceiveChatMessageEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
        
        /// <summary>
        /// The chat message that has been received.
        /// </summary>
        public ChatMessage ChatMessage { get; private set; }        
    }
}