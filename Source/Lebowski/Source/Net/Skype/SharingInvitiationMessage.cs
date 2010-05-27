namespace Lebowski.Net.Skype
{
    using System;
    
    /// <summary>
    /// Sent when a client wants to share a session using the SkypeProtocol.
    /// </summary>
    [Serializable]
    internal sealed class SharingInvitationMessage
    {
        /// <summary>
        /// Initializes a new instance of the SharingInvitationMessage class.
        /// </summary>
        /// <param name="invitationId">See <see cref="InvitationId">InvitationId</see>.</param>
        /// <param name="documentName">See <see cref="DocumentName">DocumentName</see>.</param>
        /// <param name="invitedUser">See <see cref="InvitedUser">InvitedUser</see>.</param>
        /// <param name="channel">See <see cref="Channel">Channel</see>.</param>
        public SharingInvitationMessage(int invitationId, string documentName, string invitedUser, int channel)
        {
            InvitationId = invitationId;
            DocumentName = documentName;
            InvitedUser = invitedUser;
            Channel = channel;
        }        
        
        /// <summary>
        /// An identifier uniquely identifying the invitation on the host side
        /// </summary>
        public int InvitationId { get; private set; }

        /// <summary>
        /// The name of the document that the host would like to share
        /// </summary>
        public string DocumentName { get; private set; }

        /// <summary>
        /// The name of the invited user
        /// </summary>
        public string InvitedUser { get; private set; }

        /// <summary>
        /// The channel, i.e. the connectionId to we, after the session is established, should send our
        /// data
        /// </summary>
        public int Channel { get; private set; }
    }
}