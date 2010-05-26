namespace Lebowski.Net.Multichannel
{
    using System;    
    
    /// <summary>
    /// Sent when a message is tunneled through a <see cref="MultichannelConnection">MultichannelConnection</see>.
    /// </summary>
    [Serializable]
    class MultichannelMessage
    {
        /// <summary>
        /// Initializes a new instance of the MultichannelMessage class with
        /// a channel identifier corresponding to the initiating TunneledConnection
        /// and a message containg the nested data.
        /// </summary>
        /// <param name="channelId">See <see cref="ChannelId">ChannelId</see>.</param>
        /// <param name="message">See <see cref="Message">Message</see>.</param>
        public MultichannelMessage(int channelId, object message)
        {
            ChannelId = channelId;
            Message = message;
        }
        
        /// <summary>
        /// The channel identifier, uniquely describing the TunneledConnection
        /// that initiated this message on a per MultichannelConnection level.
        /// </summary>
        public int ChannelId { get; private set; }
        
        /// <summary>
        /// The nested message that should be dispatched to the according
        /// TunneledConnection.
        /// </summary>
        public object Message { get; private set; }        
    }
}
