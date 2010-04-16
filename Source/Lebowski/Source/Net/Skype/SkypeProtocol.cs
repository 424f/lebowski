using System;
using SKYPE4COMLib;

namespace Lebowski.Net.Skype
{
	public class SkypeProtocol : ITextProtocol
	{
		private const string ApplicationName = "LEBOWSKI-01";
		private Application Application;
			
		private SKYPE4COMLib.Skype API;
		
		public SkypeProtocol()
		{
			Initialize();
			API = new SKYPE4COMLib.Skype();
			Application = API.get_Application(ApplicationName);
			Application.Create();							
			string user = API.CurrentUser.FullName;
		}
		
		public event EventHandler<EventArgs> Received;
		
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
		
		ITextConnection ITextProtocol.Connect(object settings)
		{
			Application.Connect((string)settings, true);
			Application.SendDatagram("HI", Application.SendingStreams);
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
		}
		
	}
}
