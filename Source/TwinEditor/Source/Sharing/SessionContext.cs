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

    /// <summary>
    /// Contains behavior and state of a local or collaborative session.
    /// </summary>
    public class SessionContext : ISynchronizationSession
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SessionContext));

        /// <summary>
        /// The session's current behavioral state.
        /// </summary>
        private SessionState currentState;

        /// <summary>
        /// Initializes a new instance of the SessionContext class, associated
        /// with the provided ITextContext.
        /// </summary>
        /// <param name="context">The text context this session is associated with.</param>
        public SessionContext(ITextContext context)
        {
            Context = context;
        }        

        /// <summary>
        /// Sets or gets the descriptive state of this session.
        /// </summary>
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

        /// <summary>
        /// Gets the Synchronization strategy that is used in this session.
        /// </summary>
        public ISynchronizationStrategy SynchronizationStrategy { get; internal set; }
        
        /// <summary>
        /// The text context that this session is operating on.
        /// </summary>
        public ITextContext Context { get; internal set; }

        /// <summary>
        /// The file type the document in this session currently has.
        /// </summary>
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

        /// <summary>
        /// The application connection that is used to transmit non-synchronization-
        /// specific messages.
        /// </summary>
        public IConnection ApplicationConnection { get; private set; }
        
        /// <summary>
        /// The synchronization connection, used by the <see cref="SynchronizationStrategy">SynchronizationStrategy</see>.
        /// </summary>
        public IConnection SynchronizationConnection { get; private set; }
        
        /// <summary>
        /// A unique identifier describing the current site in the session.
        /// </summary>
        public int SiteId { get; private set; }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close()
        {
            SendDisconnectionMessage();
            OnClosing(new EventArgs());
        }

        /// <summary>
        /// Establishes a shared session using an existing connection.
        /// </summary>
        /// <param name="siteId">The siteId that is used for the current session.</param>
        /// <param name="connection">The connection that we will be synchronizing over.</param>
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
        
        /// <summary>
        /// Informs other session members that we are disconnecting.
        /// </summary>
        private void SendDisconnectionMessage()
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

        /// <summary>
        /// The file name associated with the 
        /// </summary>
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

        /// <summary>
        /// Activates a new state by first unregistering the current state 
        /// and then registering the new one.
        /// </summary>
        /// <param name="state">The new state to register.</param>
        public void ActivateState(SessionState state)
        {
            if (currentState != null)
            {
                currentState.Unregister();
            }
            state.Register();
            currentState = state;
        }

        /// <summary>
        /// Sends a chat message to other connected users.
        /// </summary>
        /// <param name="message"></param>
        public void SendChatMessage(ChatMessage message)
        {
            ApplicationConnection.Send(message);
        }

        /// <summary>
        /// Executes the document associated with the session.
        /// </summary>
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

        /// <summary>
        /// Resets the session, closing associated connections and the
        /// synchronization strategy. As a result, we will have a pure local
        /// session again.
        /// </summary>
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

        /// <summary>
        /// Raises the <see cref="StateChanged">StateChanged</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        internal virtual void OnStateChanged(EventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ReceiveChatMessage">ReceiveChatMessage</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        internal virtual void OnReceiveChatMessage(ReceiveChatMessageEventArgs e)
        {
            if (ReceiveChatMessage != null)
            {
                ReceiveChatMessage(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="FileTypeChanged">FileTypeChanged</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        internal virtual void OnFileTypeChanged(EventArgs e)
        {
            if (FileTypeChanged != null)
            {
                FileTypeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="StartedExecution">StartedExecution</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        internal virtual void OnStartedExecution(StartedExecutionEventArgs e)
        {
            if (StartedExecution != null)
            {
                StartedExecution(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing">Closing</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnClosing(EventArgs e)
        {
            if (Closing != null)
            {
                Closing(this, e);
            }
        }
        
        /// <summary>
        /// Raises the <see cref="FileNameChanged">FileNameChanged</see> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnFileNameChanged(EventArgs e)
        {
            if (FileNameChanged != null)
            {
                FileNameChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when the session is closing.
        /// </summary>
        public event EventHandler<EventArgs> Closing;
        
        /// <summary>
        /// Occurs when the file name associated with the session changed.
        /// </summary>
        public event EventHandler FileNameChanged;           
        
        /// <summary>
        /// Occurs when the file type associated with the session changed.
        /// </summary>
        public event EventHandler FileTypeChanged;
                
        /// <summary>
        /// Occurs when a chat message has been received.
        /// </summary>
        public event EventHandler<ReceiveChatMessageEventArgs> ReceiveChatMessage;
        
        /// <summary>
        /// Occurs when the document associated with the session has been executed.
        /// </summary>
        public event EventHandler<StartedExecutionEventArgs> StartedExecution;
        
        /// <summary>
        /// Occurs when the session changed its state.
        /// </summary>
        public event EventHandler StateChanged;
        
             
        
    }
}