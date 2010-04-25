
using System;
using System.Windows.Forms;

using Lebowski;
using Lebowski.Net;
using Lebowski.Net.Lidgren;
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

		private void RunLocalSample()
		{
			ServerConnection s = new ServerConnection();
			System.Console.WriteLine("Server created..");
			
			ClientConnection c = new ClientConnection("localhost", ClientConnection.Port);
			Console.WriteLine("Client connected..");

			clientForm = new MainForm();
			clientForm.Text = "Client (1)";
			
			serverForm = new MainForm();			
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
			
			Console.Write("Server (s) or client (c): ");
			string choice = Console.ReadLine();
			
			IConnection connection;
			DifferentialSynchronizationStrategy sync;
			
			MainForm form = new MainForm();
			ITextContext context = new TextEditorTextContext(form.SourceCode);			
			
			if(choice == "s")
			{
				connection = new ServerConnection();	
				
				// We have to use a multichannel connection
				MultichannelConnection mcc = new MultichannelConnection(connection);
				sync = new DifferentialSynchronizationStrategy(0, context, mcc.CreateChannel());
			
				// Use 2nd channel to transport chat messages
				var chatChannel = mcc.CreateChannel();		
				form.SetChatConnection(chatChannel);
			}
			else
			{
				Console.Write("Address: ");
				string address = Console.ReadLine();
				try 
				{
					connection = new ClientConnection(address, ClientConnection.Port);
				}
				catch(ConnectionFailedException e)
				{
					Console.WriteLine("**ERROR** " + e.ToString());
					Console.ReadKey(true);
					return;
				}
				
				// We have to use a multichannel connection
				MultichannelConnection mcc = new MultichannelConnection(connection);
				sync = new DifferentialSynchronizationStrategy(1, context, mcc.CreateChannel());
			
				// Use 2nd channel to transport chat messages
				var chatChannel = mcc.CreateChannel();
				form.SetChatConnection(chatChannel);
			}
			
			
			var timer = new System.Timers.Timer(20);
			timer.Elapsed += delegate { 
				clientForm.BeginInvoke((Action)UpdateText);
			};
			timer.Enabled = true;			
			
			form.Show();	

			
			Application.Run();			
		}
		
		void UpdateText()
		{
			clientForm.label1.Text = String.Format("{0} {1}", client.State, client.HasChanged);
			serverForm.label1.Text = String.Format("{0} {1}", server.State, server.HasChanged);			
		}
		
	}
}
