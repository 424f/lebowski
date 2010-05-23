using System;

namespace Lebowski.Net.Skype
{
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
