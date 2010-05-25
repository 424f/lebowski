using System;

namespace Lebowski.Net.Skype
{
    [Serializable]
    sealed class AcceptSharingInvitationMessage
    {
        public int InvitationId { get; private set; }
        public int Channel { get; private set; }
        
        public AcceptSharingInvitationMessage(int invitationId, int channel)
        {
            InvitationId = invitationId;
            Channel = channel;
        }
    }        
}
