using System;
using Lebowski.Net.Skype;

namespace Lebowski
{
	internal sealed class Program
	{
		
		[STAThread]
		private static void Main(string[] args)
		{			
			SkypeProtocol protocol = new SkypeProtocol();
			
			Console.Write("Destination: ");
			string user = Console.ReadLine();
			if(user != "") 
			{
				protocol.Connect(user);
			}
			
			
			
			System.Windows.Forms.Application.Run();
		}
	}
}