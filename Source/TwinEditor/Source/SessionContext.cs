using System;
using Lebowski;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;
using Lebowski.TextModel;
using TwinEditor;
using TwinEditor.Messaging;
using log4net;

namespace TwinEditor
{
    public class SessionContext : ISynchronizationSession
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SessionContext));
        
        public event EventHandler StateChanged;
        public event EventHandler<ReceiveChatMessageEventArgs> ReceiveChatMessage;
        
        SessionState currentState;
        
        public SessionStates State
        {
            get { return state; }
            set
            {
                if(state != value)
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
		}
		
		public void Close()
		{
		    
		}
		
        public string FileName { get; set; }        
        
        public void ActivateState(SessionState state)
        {
            if(currentState != null)
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
		    if(e.Message is ChatMessage)
		    {
		        OnReceiveChatMessage(new ReceiveChatMessageEventArgs((ChatMessage)e.Message));
		    }
		    else if(e.Message is ExecutionResultMessage)
		    {		    		
		    	/*ChatText.Invoke((Action) delegate 
				{		    	              
			    	ExecutionResultMessage erm = (ExecutionResultMessage)e.Message;
			    	
			    	ExecutionResult result = new ExecutionResult();
			    		
			    	TabPage newPage = new TabPage(string.Format("Remote execution"));
			    	TabControl.TabPages.Add(newPage);
			    	
					ExecutionTabControl execution = new ExecutionTabControl(result);
					
					result.OnExecutionChanged(new ExecutionChangedEventArgs(erm.StandardOut));
					result.OnFinishedExecution(new FinishedExecutionEventArgs(0));
				});*/
		    }
		    else
		    {
		        Logger.WarnFormat("Received unsupported message on application connection: {0}", e.Message.GetType().Name);
		    }
		}        
        
        protected virtual void OnStateChanged(EventArgs e)
        {
            if(StateChanged != null)
            {
                StateChanged(this, e);
            }
        }
        
        protected virtual void OnReceiveChatMessage(ReceiveChatMessageEventArgs e)
        {
            if(ReceiveChatMessage != null)
            {
                ReceiveChatMessage(this, e);
            }
        }
        
    }
    
    public abstract class SessionState
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
        
        public override void Register()
        {
            Logger.Info("Registering BootstrappingState");
            
            session.State = SessionStates.Connecting;
            session.ApplicationConnection.Received += ApplicationConnectionReceived;
            
            // If we are the client, we'll say hi to the client
            if(session.SiteId == 1)
            {
                session.ApplicationConnection.Send("HI");
                Logger.Info("Sending 'HI'");
            }
        }
        
        public override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }
        
        private void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            var sync = new DifferentialSynchronizationStrategy(session.SiteId, session.Context, session.SynchronizationConnection);
            
            if(session.SiteId == 1)
            {
                if((string)e.Message != "HI BACK")
                    throw new Exception("Unexpected message");
                session.ActivateState(new SynchronizationState(session, sync));
            }
            else if(session.SiteId == 0)
            {
                if((string)e.Message != "HI")
                    throw new Exception(string.Format("Unexpected message: '{0}', should be 'HI'", e.Message));
                session.ActivateState(new SynchronizationState(session, sync));
                Logger.Info("Sending 'HI BACK'");
                session.ApplicationConnection.Send("HI BACK");
            }
        }
    }
    
    public class SynchronizationState : SessionState
    {
        private DifferentialSynchronizationStrategy sync;
        
        public SynchronizationState(SessionContext session, DifferentialSynchronizationStrategy sync) : base(session)
        {
            this.sync = sync;
        }
        
        public override void Register()
        {
            Logger.Info("Registering SynchronizationState");            
            session.State = SessionStates.Connected;
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
}
