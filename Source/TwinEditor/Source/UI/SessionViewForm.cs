
namespace TwinEditor.UI
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using Lebowski;
    using Lebowski.Synchronization;
    using Lebowski.TextModel;
    using Lebowski.Net;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using TwinEditor.FileTypes;
    using TwinEditor.Messaging;
    using TwinEditor.Sharing;
    using log4net;
    
    public partial class SessionViewForm : UserControl, ISessionView
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SessionViewForm));

        #region Context
        public SessionContext SessionContext { get; private set; }
        public ApplicationViewForm ApplicationViewForm { get; private set; }
        private TabPage tabPage;
        #endregion

        #region Events
        public event EventHandler<StateChangedEventArgs> StateChanged;
        #endregion

        #region File management members

        public bool OnDisk { get; set; }
        public bool FileModified { get; set; }
        #endregion

        #region Execution
        // Stores the exeucution view form for each user (identified by site id)
        private Dictionary<int, ExecutionViewForm> executionViewForms = new Dictionary<int, ExecutionViewForm>();
        private Dictionary<int, TabPage> executionTabs = new Dictionary<int, TabPage>();
        private int numExecutions = 0;
        #endregion

        public ITextContext Context { get; protected set; }

        public SessionViewForm(ApplicationViewForm applicationViewForm, TabPage tabPage)
        {
            InitializeComponent();
            
            TabControl.FirstClosableTabIndex = 1;

            this.ApplicationViewForm = applicationViewForm;
            this.tabPage = tabPage;
            this.OnDisk = false;
            this.FileModified = false;

            ChatText.Enabled = false;

            splitContainer.Panel2Collapsed = true;

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
            
            SessionContext.FileNameChanged += delegate{ 
                UpdateGuiState();
            };
            
            SessionContext.Context.Changed += delegate
            {
                FileModified = true;
                UpdateGuiState();
            };
            
            SessionContext.Closing += delegate
            {
                if (FileModified)
                {
                    if (MessageBox.Show(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxOnCloseMessage"), TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxOnCloseCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        applicationViewForm.SaveRequest(this);
                    }
                }
            };

            SessionContext.FileTypeChanged += delegate
            {
                Context.Invoke((Action)delegate
                {
                    SourceCode.SetHighlighting(SessionContext.FileType.Name);
                    tabPage.ImageKey = SessionContext.FileType.Name + "Image";
                });
            };

            SessionContext.StartedExecution +=
                delegate(object sender, StartedExecutionEventArgs e)
                {
                    Context.Invoke((Action)
                        delegate
                        {
                            if (!executionViewForms.ContainsKey(e.SiteId))
                            {
                                string tabTitle = string.Format("Execution (Site {0})", e.SiteId);
                                if (e.SiteId == SessionContext.SiteId)
                                {
                                    tabTitle = "Execution (Me)";
                                }

                                TabPage newPage = new TabPage(tabTitle);
                                ExecutionViewForm executionView = new ExecutionViewForm(e.ExecutionResult);
                                newPage.Controls.Add(executionView);
                                executionViewForms[e.SiteId] = executionView;
                                executionView.Dock = DockStyle.Fill;
                                TabControl.TabPages.Add(newPage);
                                newPage.Tag = e.SiteId;
                                newPage.ImageKey = "ExecutionImage";
                                TabControl.SelectedTab = newPage;
                                executionTabs[e.SiteId] = newPage;
                            }
                            else
                            {
                                executionViewForms[e.SiteId].ExecutionResult = e.ExecutionResult;
                            }
                            
                            // For own execution, let's jump to execution tab
                            if(e.SiteId == SessionContext.SiteId)
                            {
                                TabControl.SelectedTab = executionTabs[e.SiteId];
                            }
                            
                            // TODO: we should have some indicator so the local user 
                            // is aware when the a remote user sends a new execution,
                            // but automatically opening the tab is to intrusive.
                        }
                    );
                };

            SessionContext.ReceiveChatMessage += SessionContextReceiveChatMessage;

            TabControl.TabClosed += delegate(object sender, TabClosedEventArgs e)
            {
                int siteId = (int)TabControl.TabPages[e.TabIndex].Tag;
                executionViewForms.Remove(siteId);
                executionTabs.Remove(siteId);
                TabControl.TabPages.RemoveAt(e.TabIndex);
            };

            this.SetState(SessionStates.Disconnected);

            // Load ImageList for tabs
            var rm = new System.Resources.ResourceManager("TwinEditor.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            ImageList imageList = new ImageList();
            imageList.Images.Add("TextImage", (System.Drawing.Image)rm.GetObject("TextImage"));
            imageList.Images.Add("ExecutionImage", (System.Drawing.Image)rm.GetObject("ExecutionImage"));
            TabControl.ImageList = imageList;
            tabPage3.ImageKey = "TextImage";

        }

        /// <summary>
        /// Ensures that changes in the state of this session are properly
        /// represented in the user interface.
        /// </summary>
        public void UpdateGuiState()
        {
            ApplicationViewForm.UpdateGuiState();
            string modifier = FileModified ? "*" : "";
            tabPage.Text = System.IO.Path.GetFileName(SessionContext.FileName) + modifier;
            
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

            // If we're not disconnected, we have to uncollapse the right panel
            if(state == SessionStates.Disconnected)
            {
                splitContainer.Panel2Collapsed = true;
            }
            else
            {
                splitContainer.Panel2MinSize = 0;
                splitContainer.Panel2Collapsed = false;
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
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                SendChatMessage();
            }
        }

        void ChatTextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) 13)
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
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
            }
        }

        private void SendChatMessage()
        {
            if (ChatText.Text.Length == 0)
                return;
            ChatMessage message = new ChatMessage(Configuration.ApplicationSettings.Default.UserName, ChatText.Text);
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
            if (!FileModified)
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

        void SessionViewFormLoad(object sender, EventArgs e)
        {

        }

        void Panel1Paint(object sender, PaintEventArgs e)
        {

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