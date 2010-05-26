namespace TwinEditor.Sharing
{
    using Lebowski.Net;
    using Lebowski.Synchronization;
    using TwinEditor.Messaging;
    using TwinEditor.Execution;
    
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
            else if (e.Message is ChatMessage)
            {
                session.OnReceiveChatMessage(new ReceiveChatMessageEventArgs((ChatMessage)e.Message));
            }
            else if (e.Message is ExecutionResultMessage)
            {
                ExecutionResultMessage erm = (ExecutionResultMessage)e.Message;

                ExecutionResult result = new ExecutionResult();

                session.OnStartedExecution(new StartedExecutionEventArgs((int)session.ApplicationConnection.Tag, result));

                result.OnExecutionChanged(new ExecutionChangedEventArgs(erm.StandardOut));
                result.OnFinishedExecution(new FinishedExecutionEventArgs(0, erm.StandardOut));
            }
            else
            {
                Logger.WarnFormat("Received unsupported message on application connection: {0}", e.Message.GetType().Name);
                base.ApplicationConnectionReceived(sender, e);
            }            
        }
    }
}
