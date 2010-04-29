using System;
using SKYPE4COMLib;

namespace Lebowski
{
	internal sealed class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{			
			string ApplicationName = "FooBar";
			
			Skype API = new SKYPE4COMLib.Skype();
			API.Attach(8, true);
			Application app = API.get_Application(ApplicationName);
			app.Create();	
			
			Console.WriteLine("Application created..");
						
			/*var sendStream = app.SendingStreams[app.SendingStreams.Count];
			var receiveStream = app.ReceivedStreams[app.ReceivedStreams.Count];*/
			
			API.ApplicationStreams += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{
				Console.WriteLine("Application stream..");
			};
			
			API.ApplicationConnecting += delegate(Application pApp, UserCollection pUsers)
			{
				Console.Write("Connecting: " + pApp.Name + ":: ");
				for(int i = 1; i <= pUsers.Count; ++i)
				{
					Console.Write(pUsers[i].Handle + " ");
				}
				Console.WriteLine();
				
				
				if(pUsers.Count == 1)
				{
					Console.WriteLine("Connecting...");
				}
				
				if(pUsers.Count == 0)
				{
					Console.WriteLine("Waiting for accept...");
					app.SendingStreams[1].Write("ROFL");
				}
			};
			
			API.ApplicationReceiving += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{ 
				if(pStreams.Count == 0)
					return;
				if(pApp.ReceivedStreams[1].DataLength == 0)
					return;
				string received = pApp.ReceivedStreams[1].Read();
				Console.WriteLine("RECV " + received);
			};
			
			/*while(true)
			{
				string txt = Console.ReadLine();
				sendStream.SendDatagram(txt);
			}*/
			
			Console.Write("Destination: ");
			string user = Console.ReadLine();
			app.Connect(user, true);
			
			
			Console.ReadKey(true);
		}
	}
}