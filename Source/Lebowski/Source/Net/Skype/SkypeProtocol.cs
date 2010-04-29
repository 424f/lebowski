using System;
using System.Text;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

namespace Lebowski.Net.Skype
{
	public class SkypeProtocol : ICommunicationProtocol
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		private const string ApplicationName = "LEBOWSKI-01";
		private Application Application;
			
		private SKYPE4COMLib.Skype API;
		
		public SkypeProtocol()
		{
			//Initialize();
			//API = new SKYPE4COMLib.Skype();
			
			//Application = API.get_Application(ApplicationName);
			//Application.Create();							
			/*string user = API.CurrentUser.FullName;
			
			var friends = API.Friends;
			
			for(int i = 1; i <= friends.Count; ++i)
			{
				var friend = friends[i];
				if(friend.OnlineStatus != TOnlineStatus.olsOffline)
				{
					Console.WriteLine(friend.FullName + " / " + friend.Handle + " / " + friend.DisplayName + " / " + friend.OnlineStatus);
				}
			}*/
			
			//Application.Connect(username, true);
			//Application.ConnectableUsers.Add(username);
		}
		
		public string Name {
			get { return "Skype API"; }
		}
		
		public static void Initialize()
		{
			//String username = "foo";
			//foreach(User user in API.SearchForUsers(username))
			//{
			//	Console.WriteLine(user.FullName);
			//}
			//s.SendMessage(username, "testing");

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
			throw new NotImplementedException();
		}
		
		
	}
}
