namespace Lebowski.Net.Skype
{
    using System;
    
    /// <summary>
    /// Sent when a client accepts a SharingInvitationMessage using
    /// the SkypeProtocol.
    /// </summary>
    [Serializable]
    sealed class AcceptSharingInvitationMessage
    {
        /// <summary>
        /// Initializes a new instance of the AcceptSharingInvitationMessage.
        /// </summary>
        /// <param name="invitationId">See <see cref="InvitationId">InvitationId</see>.</param>
        /// <param name="channel">See <see cref="Channel">Channel</see>.</param>
        public AcceptSharingInvitationMessage(int invitationId, int channel)
        {
            InvitationId = invitationId;
            Channel = channel;
        }

        /// <summary>
        /// An identifier that uniquely identifies the invitation that was accepted.
        /// </summary>
        public int InvitationId { get; private set; }
        
        /// <summary>
        /// The channel within the skype protocol that should be used to
        /// communicate with the accepting client.
        /// </summary>
        public int Channel { get; private set; }        
    }
}