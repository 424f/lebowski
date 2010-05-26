namespace TwinEditor.Sharing
{
    using System;
    using Lebowski;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;
    using Lebowski.Net.Multichannel;
    using Lebowski.TextModel;
    using TwinEditor;
    using TwinEditor.Messaging;
    using TwinEditor.FileTypes;
    using TwinEditor.Execution;
    using log4net;

    public class SessionContext : ISynchronizationSession
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SessionContext));

        public event EventHandler StateChanged;
        public event EventHandler<ReceiveChatMessageEventArgs> ReceiveChatMessage;
        public event EventHandler FileTypeChanged;
        public event EventHandler<StartedExecutionEventArgs> StartedExecution;
        public event EventHandler<EventArgs> Closing;
        public event EventHandler FileNameChanged;

        SessionState currentState;

        public SessionStates State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnStateChanged(new EventArgs());
                }
                state = value;
            }
        }
        private SessionStates state;

        public DifferentialSynchronizationStrategy SynchronizationStrategy { get; set; }
        public ITextContext Context { get; set; }

        public IFileType FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;
                OnFileTypeChanged(new EventArgs());
            }
        }
        private IFileType fileType;

        public IConnection ApplicationConnection { get; private set; }
        public IConnection SynchronizationConnection { get; private set; }
        public int SiteId { get; private set; }

        public SessionContext(ITextContext context)
        {
            Context = context;
        }

        public void EstablishSharedSession(int siteId, IConnection connection)
        {
            MultichannelConnection mcc = new MultichannelConnection(connection);
            SynchronizationConnection = mcc.CreateChannel();
            ApplicationConnection = mcc.CreateChannel();

            State = SessionStates.Disconnected;
            SiteId = siteId;
            ActivateState(new BootstrappingState(this));

            // TODO: this has to be altered when support for multiple sites is added
            int otherSiteId = siteId == 0 ? 1 : 0;
            // We store the the connection's site id per connection
            ApplicationConnection.Tag = otherSiteId;
            SynchronizationConnection.Tag = otherSiteId;
        }

        public void Close()
        {
            Disconnect();
            OnClosing(new EventArgs());
        }
        
        public void Disconnect()
        {
            if (State != SessionStates.Disconnected)
            {
                try
                {
                    ApplicationConnection.Send(new CloseSessionMessage());
                }
                catch(Exception e)
                {
                    Logger.WarnFormat("When trying to send CloseSessionMessage, an exception occurred: {0}", e);
                }
            }            
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnFileNameChanged(new EventArgs());
            }
        }
        private string fileName;

        public void ActivateState(SessionState state)
        {
            if (currentState != null)
            {
                currentState.Unregister();
            }

            state.Register();

            currentState = state;
        }

        public void SendChatMessage(ChatMessage message)
        {
            ApplicationConnection.Send(message);
        }

        public void Execute()
        {
            ExecutionResult result = new ExecutionResult();

            OnStartedExecution(new StartedExecutionEventArgs(SiteId, result));

            FileType.Execute(Context.Data, result);

            // When execution is finished, propagate result to other users
            result.FinishedExecution += delegate(object o, FinishedExecutionEventArgs fe)
            {
                if (State == SessionStates.Connected)
                {
                    ApplicationConnection.Send(new Messaging.ExecutionResultMessage(fe.ReturnCode, fe.StandardOut));
                }
            };
        }

        public void Reset()
        {
            ActivateState(new SessionState(this));
            if(SynchronizationStrategy != null)
            {
                SynchronizationStrategy.Close();
                SynchronizationStrategy = null;
            }
            if(ApplicationConnection != null)
            {
                ApplicationConnection.Close();
                ApplicationConnection = null;                
            }
            if(SynchronizationConnection != null)
            {
                SynchronizationConnection.Close();
                SynchronizationConnection = null;
            }

            State = SessionStates.Disconnected;
        }

        internal virtual void OnStateChanged(EventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        }

        internal virtual void OnReceiveChatMessage(ReceiveChatMessageEventArgs e)
        {
            if (ReceiveChatMessage != null)
            {
                ReceiveChatMessage(this, e);
            }
        }

        internal virtual void OnFileTypeChanged(EventArgs e)
        {
            if (FileTypeChanged != null)
            {
                FileTypeChanged(this, e);
            }
        }

        internal virtual void OnStartedExecution(StartedExecutionEventArgs e)
        {
            if (StartedExecution != null)
            {
                StartedExecution(this, e);
            }
        }
        
        protected virtual void OnClosing(EventArgs e)
        {
            if (Closing != null)
            {
                Closing(this, e);
            }
        }
        
        protected virtual void OnFileNameChanged(EventArgs e)
        {
            if (FileNameChanged != null)
            {
                FileNameChanged(this, e);
            }
        }

    }
}