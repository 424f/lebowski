
using System;
using System.Windows.Forms;

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
			ApplicationUtil.Initialize();
			Program prog = new Program();
			prog.Run();
		}
		
		MainForm clientForm;
		MainForm serverForm;
		
		DifferentialSynchronizationStrategy server;
		DifferentialSynchronizationStrategy client;
		
		private void Run()
		{			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);				
			
			MainForm form = new MainForm(new Controller());
			form.Show();	

			
			Application.Run();			
		}
		
		void UpdateText()
		{
			//clientForm.label1.Text = String.Format("{0} {1}", client.State, client.HasChanged);
			//serverForm.label1.Text = String.Format("{0} {1}", server.State, server.HasChanged);			
		}
		
	}
}
