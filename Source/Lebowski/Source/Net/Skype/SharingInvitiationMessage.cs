
namespace Lebowski.Net.Skype
{
    using System;
    [Serializable]
    sealed class SharingInvitationMessage
    {
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

        public SharingInvitationMessage(int invitationId, string documentName, string invitedUser, int channel)
        {
            InvitationId = invitationId;
            DocumentName = documentName;
            InvitedUser = invitedUser;
            Channel = channel;
        }
    }
}