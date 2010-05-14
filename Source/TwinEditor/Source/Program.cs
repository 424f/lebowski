using System;
using System.Windows.Forms;
using System.Configuration;

using Lebowski;
using Lebowski.Net;
using Lebowski.Net.Tcp;
using Lebowski.Net.Lidgren;
using Lebowski.TextModel;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace TwinEditor
{
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			log4net.Config.BasicConfigurator.Configure();
			Program prog = new Program();
			prog.Run();
		}
		
		private void SetupConfiguration()
		{		    
			// Set up configuration if necessary
			var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		    
			
			// Retrieve username from system if none has been set explicitly
			string userName = c.AppSettings.Settings["UserName"] != null ? c.AppSettings.Settings["UserName"].Value : null;
			if(userName == null || userName == "")
			{
			    userName = Environment.UserName;
			}
			c.AppSettings.Settings["UserName"].Value = userName;
			
			// Save configuration and refresh
			c.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			
			System.Console.WriteLine("Welcome, {0}", c.AppSettings.Settings["UserName"].Value);
		}
		
		private void Run()
		{
		    ApplicationUtil.Initialize();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);				
			
			SetupConfiguration();
			
			// Display main form
			MainForm form = new MainForm(new Controller());
			form.Show();

			
			Application.Run();			
		}		
	}
}
