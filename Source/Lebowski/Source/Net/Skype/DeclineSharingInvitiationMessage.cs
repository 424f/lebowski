namespace Lebowski.Net.Skype
{
    using System;
    
    /// <summary>
    /// Sent when a client rejects a SharingInvitationMessage using
    /// the SkypeProtocol.
    /// </summary>
    [Serializable]
    internal sealed class DeclineSharingInvitationMessage
    {
        /// <summary>
        /// Initializes a new instance of the DeclineSharingInvitationMessage.
        /// </summary>
        /// <param name="invitationId">See <see cref="InvitationId">InvitationId</see>.</param>     
        public DeclineSharingInvitationMessage(int invitationId)
        {
            InvitationId = invitationId;
        }
        
        /// <summary>
        /// An identifier that uniquely identifies the invitation that was rejected.
        /// </summary>        
        public int InvitationId { get; private set; }        
    }
}