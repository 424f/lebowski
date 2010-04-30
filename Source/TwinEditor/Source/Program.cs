
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
			Program prog = new Program();
			prog.Run();
		}
		
		MainForm clientForm;
		MainForm serverForm;
		
		DifferentialSynchronizationStrategy server;
		DifferentialSynchronizationStrategy client;

		private void RunLocalSample()
		{
			ServerConnection s = new ServerConnection();
			System.Console.WriteLine("Server created..");
			
			ClientConnection c = new ClientConnection("localhost", ClientConnection.Port);
			Console.WriteLine("Client connected..");

			Controller clientController = new Controller();
			clientForm = new MainForm(clientController);
			clientForm.Text = "Client (1)";
			
			Controller serverController = new Controller();
			serverForm = new MainForm(serverController);			
			serverForm.Text = "Server (0)";

			IConnection serverConnection = s; // new LocalConnection();
			IConnection clientConnection = c; // new LocalConnection();
			//LocalProtocol.Connect(serverConnection, clientConnection);
			
			ITextContext serverContext = new TextEditorTextContext(serverForm.SourceCode);
			ITextContext clientContext = new TextEditorTextContext(clientForm.SourceCode);
			
			server = new DifferentialSynchronizationStrategy(0, serverContext, serverConnection);
			client = new DifferentialSynchronizationStrategy(1, clientContext, clientConnection);				
			
			var timer = new System.Timers.Timer(20);
			timer.Elapsed += delegate { 
				clientForm.BeginInvoke((Action)UpdateText);
			};
			timer.Enabled = true;
			
			clientForm.Show();
			serverForm.Show();			
		}
		
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
