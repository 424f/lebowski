using System;
using System.Text;
using System.Collections.Generic;
using Lebowski.Synchronization.DifferentialSynchronization;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

// TODO: remove dependencies on specific synchronization strategy

namespace Lebowski.Net.Skype
{
	public class SkypeProtocol : ICommunicationProtocol
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		private const string ApplicationName = "LEBOWSKI-01";
		private static Application Application;
			
		private static SKYPE4COMLib.Skype API;
		
		private static bool isInitialized = false;
		
		public List<string> friends = new List<string>();
		
		public SkypeProtocol()
		{
			Initialize();

			/*string user = API.CurrentUser.FullName;*/
			
			UpdateFriends();
			
			//Application.Connect(username, true);
			//Application.ConnectableUsers.Add(username);
		}
		
		protected void UpdateFriends()
		{
			this.friends.Clear();
			
			var friends = API.Friends;
			
			for(int i = 1; i <= friends.Count; ++i)
			{
				var friend = friends[i];
				if(friend.OnlineStatus != TOnlineStatus.olsOffline)
				{
					Console.WriteLine(friend.FullName + " / " + friend.Handle + " / " + friend.DisplayName + " / " + friend.OnlineStatus);
				}
				this.friends.Add(friend.Handle);
			}			
			
		}
		
		public string Name {
			get { return "Skype API"; }
		}
		
		public static void Initialize()
		{
			if(isInitialized)
				return;

			API = new SKYPE4COMLib.Skype();
			
			Application = API.get_Application(ApplicationName);
			Application.Create();										
			
			isInitialized = true;

		}
		
		object Connect(object settings)
		{
			Application.Connect((string)settings, true);
			Application.SendDatagram("HI", Application.SendingStreams);
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
		}
		
		public void Share(ISessionContext session)
		{
			Initialize();
			SkypeShareForm form = new SkypeShareForm(this);
			form.Submit += delegate
			{  
				// Send an invitation
				Application.Connect(form.SelectedUser, true);
				string connectionName = Guid.NewGuid().ToString();
				
				
				SkypeConnection connection = new SkypeConnection(API, Application);
				
				// We have to use a multichannel connection
				MultichannelConnection mcc = new MultichannelConnection(connection);
				var sync = new DifferentialSynchronizationStrategy(0, session.Context, mcc.CreateChannel());
				var applicationChannel = mcc.CreateChannel();		
				session.StartSession(sync, connection, applicationChannel);				
			};
			form.ShowDialog();
		}
		
		public bool CanShare
		{
			get { return true; }
		}
		
		public bool CanParticipate
		{
			get { return true; }
		}		
	}
}
