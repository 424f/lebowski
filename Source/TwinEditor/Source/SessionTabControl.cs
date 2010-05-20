using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.UI.FileTypes;
using log4net;

namespace TwinEditor
{
	public partial class SessionTabControl : UserControl, ISessionContext
	{	
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));
		
		public event EventHandler<StateChangedEventArgs> StateChanged;
		
		public SessionState State { get; private set; }
		public string FileName { get; set; }
		public bool OnDisk { get; set; }
		public bool FileModified { get; set; }
		private IFileType fileType;
		public IFileType FileType
		{
			get { return fileType; }
			set
			{
				fileType = value;
				// ICSharpCode.TextEditor.dll does include a seperate syntax highlight definition (.xshd) for python and text.
				// However python-mode.xshd is identical to boo.xshd
				SourceCode.SetHighlighting(fileType.Name);
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
	
		public SessionTabControl()
		{
			this.OnDisk = false;
			this.FileModified = false;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			ChatText.Enabled = false;
			
			// Create a text context for the source code editor
			Context = new TextEditorTextContext(SourceCode);
			
			this.SetState(SessionState.Disconnected);
		}
		
		public void Close()
		{
			
		}
		
		public void SetState(SessionState state)
		{
			if (State != state)
			{
				OnStateChanged(new StateChangedEventArgs(state));
			}
		    State = state;
		    switch(State)
		    {
		        case SessionState.Disconnected:
		    		this.ChangeStatus("Disconnected", false, false);
		            break;
		        case SessionState.Connecting:
		            this.ChangeStatus("Connecting...", true, false);
		            break;
		        case SessionState.Connected:
		           	this.ChangeStatus("Connected", false, false);
		            break;
		        case SessionState.AwaitingConnection:
		            this.ChangeStatus("Awaiting...", true, true);
		            break;
		    }
		    
		}
		
		private void ChangeStatus(String status, bool spinner, bool cancellable)
		{
			if (this.connectionStatusLabel.InvokeRequired)
		    		{
			            this.connectionStatusLabel.Invoke((Action) delegate
		    			                                  {
		    			                                  	this.connectionStatusLabel.Text = status;
		    			                                  	this.connectionStatusPicture.Visible = spinner;
		            										//this.connectionStopWaitingButton.Visible = cancellable;
		    			                                  });
		            }
		            else
		            {
                      	this.connectionStatusLabel.Text = status;
                      	this.connectionStatusPicture.Visible = spinner;
						//this.connectionStopWaitingButton.Visible = cancellable;		            	
		            }
		}
		
		
		protected void OnStateChanged(StateChangedEventArgs e)
		{
			if (StateChanged != null)
			{
				StateChanged(this, e);
			}
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
			
			ChatText.Invoke((Action)delegate { ChatText.Enabled = true; });
			
			// When we receive a chat message, display it in the text field
            ApplicationConnection.Received += delegate(object sender, ReceivedEventArgs e) {
				string s = (string)e.Message;
				ChatText.Invoke((Action)delegate { AddChatMessage(s); });
			};		    
			this.SetState(SessionState.Connected);
		}
		
		public void AwaitingSession()
		{
			this.SetState(SessionState.AwaitingConnection);
		}
		
		public void CloseSession()
		{
			ChatText.Enabled = false;
			this.SetState(SessionState.Disconnected);
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
		
		
		void SourceCodeTextChanged(object sender, System.EventArgs e)
		{
			if(!FileModified) 
			{
				FileModified = true;
				((TabPage)this.Parent).Text += " *";
			}
		}
			
		void ConnectionStopWaitingButtonClick(object sender, EventArgs e)
		{
			// TODO: if elegantly possible for all protocols, consider implementing this.
		}
	}
	
	public sealed class StateChangedEventArgs : EventArgs
	{
		//public IConnection Connection { get; private set; }
		//public ISessionContext Session { get; private set; }
		public SessionState State { get; private set; }
		
		public StateChangedEventArgs(SessionState state)
		{
			State = state;
		}
	}
}
