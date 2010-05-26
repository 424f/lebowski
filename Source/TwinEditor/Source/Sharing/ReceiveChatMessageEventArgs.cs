namespace TwinEditor.Sharing
{
    using System;
    using TwinEditor.Messaging;
    
    public sealed class ReceiveChatMessageEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; private set; }

        public ReceiveChatMessageEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}
