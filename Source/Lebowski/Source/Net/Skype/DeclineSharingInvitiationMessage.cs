
namespace Lebowski.Net.Skype
{
    using System;
    [Serializable]
    sealed class DeclineSharingInvitationMessage
    {
        public int InvitationId { get; private set; }
        
        public DeclineSharingInvitationMessage(int invitationId)
        {
            InvitationId = invitationId;
        }
    }    
}
