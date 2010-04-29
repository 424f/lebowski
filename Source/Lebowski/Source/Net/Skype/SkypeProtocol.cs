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
			Initialize();
			API = new SKYPE4COMLib.Skype();
			
			Application = API.get_Application(ApplicationName);
			Application.Create();							
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
		
	}
	
	public class SkypeConnection : IConnection
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		public event EventHandler<ReceivedEventArgs> Received;
		private Application application;
		private SKYPE4COMLib.Skype api;
		
		public SkypeConnection(SKYPE4COMLib.Skype api, Application application)
		{
			this.api = api;
			this.application = application;
			
			api.ApplicationDatagram += delegate(Application app, ApplicationStream stream, string text) { 
				if(app != this.application)
				{
					return;
				}
				
				// Decode message
				Logger.Info(string.Format("Received skype datagram: {0}", text));
				try
				{
					byte[] buffer = Convert.FromBase64String(text);
					OnReceived(new ReceivedEventArgs(NetUtils.Deserialize(buffer)));
				}
				catch(Exception e)
				{
					Logger.Error("Received ill-formed skype datagram", e);
				}
			};			
		}
		
		public void Send(object o)
		{
			byte[] buffer = NetUtils.Serialize(o);
			string base64buffer = Convert.ToBase64String(buffer);
			
			application.SendDatagram(base64buffer, application.Streams);
		}
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
	}
}
