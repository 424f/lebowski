﻿using System;
using SKYPE4COMLib;

namespace Lebowski
{
	internal sealed class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Console.Write("Destination: ");
			string user = Console.ReadLine();
			
			string ApplicationName = "FooBar";
			
			Skype API = new SKYPE4COMLib.Skype();
			Application app = API.get_Application(ApplicationName);
			app.Create();	
			
			app.Connect(user, true);
			
			var sendStream = app.SendingStreams[app.SendingStreams.Count];
			var receiveStream = app.ReceivedStreams[app.ReceivedStreams.Count];
			
			API.ApplicationStreams += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{
				Console.WriteLine("Application stream..");
			};
			
			API.ApplicationConnecting += delegate(Application pApp, UserCollection pUsers)
			{
				if(pApp.Streams.Count == 0)
				{
					Console.WriteLine("Connecting...");
				}
				
				if(pApp.Streams.Count == 1)
				{
					Console.WriteLine("Waiting for accept...");
					app.Streams[1].Write("ROFL");
				}
			};
			
			API.ApplicationReceiving += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{ 
				if(pStreams.Count == 0)
					return;
				if(pStreams[1].DataLength == 0)
					return;
				string received = receiveStream.Read();
				Console.WriteLine("RECV " + received);
			};
			
			/*while(true)
			{
				string txt = Console.ReadLine();
				sendStream.SendDatagram(txt);
			}*/
			Console.ReadKey(true);
		}
	}
}
