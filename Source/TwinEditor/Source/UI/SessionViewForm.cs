﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.FileTypes;
using TwinEditor.Messaging;
using log4net;

namespace TwinEditor.UI
{
	public partial class SessionViewForm : UserControl, ISessionView
	{	
	    public SessionContext SessionContext
	    {
	        get;
	        private set;
	    }
	    
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SessionViewForm));
		
		public event EventHandler<StateChangedEventArgs> StateChanged;
		
		public string FileName
		{
		    get { return fileName; }
		    set
		    {
		        fileName = value;
		        tabPage.Text = fileName;
		    }
	    }
		private string fileName;
	
		public bool OnDisk { get; set; }
		public bool FileModified { get; set; }
		
		private TabPage tabPage;
		
		private IFileType fileType;
		public IFileType FileType
		{
			get { return fileType; }
			set
			{
				fileType = value;
				// ICSharpCode.TextEditor.dll does include a seperate syntax highlight definition (.xshd) for python and text.
				// However, python-mode.xshd is identical to boo.xshd
				SourceCode.SetHighlighting(fileType.Name);
			}
		}
		public int NumExecutions { get; protected set; }
		public void IncrementNumExections()
		{
			NumExecutions += 1;
		}
		
		public ITextContext Context { get; protected set; }
		public ApplicationViewForm ApplicationViewForm { get; private set; }
		
		public DifferentialSynchronizationStrategy SynchronizationStrategy { get; protected set; }
	
		public SessionViewForm(ApplicationViewForm applicationViewForm, TabPage tabPage)
		{
			InitializeComponent();
			
			this.ApplicationViewForm = applicationViewForm;
			this.tabPage = tabPage;
			this.OnDisk = false;
			this.FileModified = false;
			
			ChatText.Enabled = false;
			
			// Create a text context for the source code editor
			Context = new TextEditorTextContext(SourceCode);
			
			SessionContext = new SessionContext(Context);
			SessionContext.StateChanged += delegate(object sender, EventArgs e)
			{
			    Context.Invoke((Action)delegate 
                {
                    SessionContextStateChanged(sender, e);
                });
			};
			
			SessionContext.ReceiveChatMessage += SessionContextReceiveChatMessage;
			
			this.SetState(SessionStates.Disconnected);
		}
		
		public void Close()
		{
			
		}
		
		public void UpdateGuiState()
		{
            ApplicationViewForm.UpdateGuiState();		    
            ChatText.Enabled = SessionContext.State == SessionStates.Connected;
		}
		
		public void SetState(SessionStates state)
		{
		    Logger.InfoFormat("State changed to {0}", state);
		    
			OnStateChanged(new StateChangedEventArgs(state));
			
		    switch(state)
		    {
		        case SessionStates.Disconnected:
		    		this.ChangeStatus(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "StatusDisconnected"), false, false);
		            break;
		        case SessionStates.Connecting:
		            this.ChangeStatus(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "StatusConnecting"), true, false);
		            break;
		        case SessionStates.Connected:
		           	this.ChangeStatus(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "StatusConnected"), false, false);
		            break;
		        case SessionStates.AwaitingConnection:
		            this.ChangeStatus(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "StatusAwaiting"), true, true);
		            break;
		    }
		    UpdateGuiState();
		    
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
            ChatMessage message = new ChatMessage(Environment.UserName, ChatText.Text);
            SessionContext.SendChatMessage(message);
			AddChatMessage(message);
			ChatText.Text = "";		                
		}
		
		private void AddChatMessage(ChatMessage msg)
		{
		    ChatHistory.AppendText(msg.UserName + ": " + msg.Message + Environment.NewLine);
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
		
		public void SessionContextStateChanged(object o, EventArgs e)
		{
		    SetState(SessionContext.State);
		}
		
		public void SessionContextReceiveChatMessage(object o, ReceiveChatMessageEventArgs e)
		{
		    Context.Invoke((Action)delegate
		    {
		        AddChatMessage(e.ChatMessage);
            });
		}
	}
	
	public sealed class StateChangedEventArgs : EventArgs
	{
		//public IConnection Connection { get; private set; }
		//public ISessionContext Session { get; private set; }
		public SessionStates State { get; private set; }
		
		public StateChangedEventArgs(SessionStates state)
		{
			State = state;
		}
	}
}
