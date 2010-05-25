namespace TwinEditor
{
    using System;
    using Lebowski;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;
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
            
            ApplicationConnection.Received += ApplicationConnectionReceived;
            
            // TODO: this has to be altered when support for multiple sites is added
            int otherSiteId = siteId == 0 ? 1 : 0;
            // We store the the connection's site id per connection
            ApplicationConnection.Tag = otherSiteId;
            SynchronizationConnection.Tag = otherSiteId;
        }
        
        public void Close()
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
        
        public string FileName { get; set; }        
        
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
        
        private void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            if (e.Message is ChatMessage)
            {
                OnReceiveChatMessage(new ReceiveChatMessageEventArgs((ChatMessage)e.Message));
            }
            else if (e.Message is ExecutionResultMessage)
            {                    
                ExecutionResultMessage erm = (ExecutionResultMessage)e.Message;
                
                ExecutionResult result = new ExecutionResult();
                
                OnStartedExecution(new StartedExecutionEventArgs((int)ApplicationConnection.Tag, result));
                
                result.OnExecutionChanged(new ExecutionChangedEventArgs(erm.StandardOut));
                result.OnFinishedExecution(new FinishedExecutionEventArgs(0, erm.StandardOut));
            }
            else
            {
                Logger.WarnFormat("Received unsupported message on application connection: {0}", e.Message.GetType().Name);
            }
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
            SynchronizationStrategy.Close();
            ApplicationConnection.Close();
            SynchronizationConnection.Close();
            
            SynchronizationConnection = null;
            ApplicationConnection = null;
            SynchronizationStrategy = null;      
            
            State = SessionStates.Disconnected;
        }
        
        protected virtual void OnStateChanged(EventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        }
        
        protected virtual void OnReceiveChatMessage(ReceiveChatMessageEventArgs e)
        {
            if (ReceiveChatMessage != null)
            {
                ReceiveChatMessage(this, e);
            }
        }
        
        protected virtual void OnFileTypeChanged(EventArgs e)
        {
            if (FileTypeChanged != null)
            {
                FileTypeChanged(this, e);
            }
        }
        
        protected virtual void OnStartedExecution(StartedExecutionEventArgs e)
        {
            if (StartedExecution != null)
            {
                StartedExecution(this, e);
            }
        }
        
    }
    
    public class SessionState
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(SessionState));
        protected SessionContext session;
        
        public SessionState(SessionContext session)
        {
            this.session = session;    
        }
        
        public virtual void Register()
        {
            
        }
        
        public virtual void Unregister()
        {
            
        }
        
        protected virtual void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            
        }
    }
    
    /// <summary>
    /// Will bootstrap a session, including:
    ///     - Initial synchronization of contents
    ///     - Deciding which synchronization strategy to use
    /// </summary>
    /// 
    // TODO: meaningful bootstrapping :)
    public class BootstrappingState : SessionState
    {
        public BootstrappingState(SessionContext session) : base(session) {}
        
        bool receivedHandshake = false;
        
        public override void Register()
        {
            Logger.Info("Registering BootstrappingState");
            
            session.State = SessionStates.Connecting;
            session.ApplicationConnection.Received += ApplicationConnectionReceived;
            
            // If we are the client, we'll say hi to the client  
            session.ApplicationConnection.Send("HI 0");
            Logger.Info("Sending 'HI'");
        }
        
        public override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }
        
        protected override void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            session.SynchronizationStrategy = new DifferentialSynchronizationStrategy(session.SiteId, session.Context, session.SynchronizationConnection);
            
            if ((string)e.Message == "HI 0")
            {
                session.ApplicationConnection.Send("HI 1");
                session.ActivateState(new SynchronizationState(session));
            } 
            else if ((string)e.Message == "HI 1")
            {
                session.ActivateState(new SynchronizationState(session));
            }
            
        }
    }
    
    public class SynchronizationState : SessionState
    {
        public SynchronizationState(SessionContext session) : base(session)
        {

        }
        
        public override void Register()
        {
            Logger.Info("Registering SynchronizationState");            
            session.State = SessionStates.Connected;
            session.ApplicationConnection.Received += ApplicationConnectionReceived;
        }
        
        public override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;            
        }
        
        protected override void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            if (e.Message is CloseSessionMessage)
            {                
                session.Reset();
            }
            else
            {
                base.ApplicationConnectionReceived(sender, e);
            }
        }
    }
    
    public sealed class ReceiveChatMessageEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; private set; }
        
        public ReceiveChatMessageEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
    
    public sealed class StartedExecutionEventArgs : EventArgs
    {
        /// <summary>
        /// The result object dispatches events about this execution
        /// </summary>
        public ExecutionResult ExecutionResult { get; private set; }
        /// <summary>
        /// The id of the site where this execution originated
        /// </summary>
        public int SiteId { get; private set; }
        
        public StartedExecutionEventArgs(int siteId, ExecutionResult executionResult)
        {
            ExecutionResult = executionResult;
            SiteId = siteId;
        }
    }
}
