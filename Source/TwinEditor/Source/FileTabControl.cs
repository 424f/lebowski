using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.UI.FileTypes;

namespace TwinEditor
{
	public partial class FileTabControl : UserControl, ISessionContext
	{
		public string FileName { get; set; }
		public bool OnDisk { get; set; }
		private IFileType fileType;
		public IFileType FileType
		{
			get { return fileType; }
			set
			{
				fileType = value;
				// TODO: build new .dll to include python syntax highlight definition (.xshd)
				// SourceCode.SetHighlighting(fileType.Name);
				SourceCode.SetHighlighting("Boo");
			}
		}
		public int NumExecutions { get; protected set; }
		public void IncrementNumExections()
		{
			NumExecutions += 1;
		}
		
		public ITextContext Context { get; protected set; }
		public IConnection ApplicationConnection { get; protected set; }
		public DifferentialSynchronizationStrategy SynchronizationStrategy { get; protected set; }
	
		public FileTabControl()
		{
			this.OnDisk = false;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			// Create a text context for the source code editor
			Context = new TextEditorTextContext(SourceCode);
		}
		
		public void Close()
		{
			
		}
		
		public void StartSession(DifferentialSynchronizationStrategy strategy, IConnection applicationConnection)
		{
		    if(SynchronizationStrategy != null)
		    {
		        throw new InvalidOperationException("synchronization stategy has already been set.");
		    }
		    if(ApplicationConnection != null)
		    {
		        throw new InvalidOperationException("application connection has already been set.");
		    }
		    		    
			SynchronizationStrategy = strategy;
			ApplicationConnection = applicationConnection;
			
			// When we receive a chat message, display it in the text field
            ApplicationConnection.Received += delegate(object sender, ReceivedEventArgs e) {
				string s = (string)e.Message;
				ChatText.Invoke((Action)delegate { AddChatMessage(s); });
			};		    
			
		}
		
		public void CloseSession()
		{
			
		}
		
		
		void ChatTextKeyDown(object sender, KeyEventArgs e)
		{
            if(e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				SendChatMessage();
			}			
		}
		
		void ChatTextKeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) 13)
			{
				e.Handled = true;
			}
			else
			{
				base.OnKeyPress(e);
			}
		}
		
		void ChatTextKeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				e.Handled = true;
			}
		}
		
		private void SendChatMessage()
		{
            if(ChatText.Text.Length == 0)
				return;
            string message = Environment.UserName + ": " + ChatText.Text;
			ApplicationConnection.Send(message);
			AddChatMessage(message);
			ChatText.Text = "";		    
		}
		
		private void AddChatMessage(string text)
		{
		    ChatHistory.AppendText(text + Environment.NewLine);
		}
		
		void ChatTextTextChanged(object sender, EventArgs e)
		{
			
		}
	}
}
