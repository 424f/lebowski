
using System;
using System.Windows.Forms;

using Lebowski;
using Lebowski.Net;
using Lebowski.TextModel;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace TwinEditor
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
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
			
			clientForm = new MainForm();
			clientForm.Text = "Client (1)";
			
			serverForm = new MainForm();			
			serverForm.Text = "Server (0)";

			LocalConnection serverConnection = new LocalConnection();
			LocalConnection clientConnection = new LocalConnection();
			LocalProtocol.Connect(serverConnection, clientConnection);
			
			ITextContext serverContext = new TextBoxTextContext(serverForm.SourceCode);
			ITextContext clientContext = new TextBoxTextContext(clientForm.SourceCode);
			
			server = new DifferentialSynchronizationStrategy(0, serverContext, clientConnection);
			server.EnableAutoFlush = true;
			client = new DifferentialSynchronizationStrategy(1, clientContext, serverConnection);				
			client.EnableAutoFlush = true;
			
			var timer = new System.Timers.Timer(20);
			timer.Elapsed += delegate { 
				clientForm.BeginInvoke((Action)UpdateText);
			};
			timer.Enabled = true;
			
			clientForm.Show();
			serverForm.Show();
			
			Application.Run();			
		}
		
		void UpdateText()
		{
			clientForm.label1.Text = String.Format("{0} {1}", client.State, client.HasChanged);
			serverForm.label1.Text = String.Format("{0} {1}", server.State, server.HasChanged);			
		}
		
	}
}
