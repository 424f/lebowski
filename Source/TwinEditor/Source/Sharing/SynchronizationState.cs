namespace TwinEditor.Sharing
{
    using Lebowski.Net;
    using Lebowski.Synchronization;
    using TwinEditor.Messaging;
    using TwinEditor.Execution;
    
    /// <summary>
    /// This makes up the major part of a session: it provides synchronization
    /// among multiple sites, using a ISynchronizationStrategy to achieve this
    /// goal.
    /// </summary>
    internal class SynchronizationState : SessionState
    {
        /// <summary>
        /// Initializes a new instance of the SynchronizationState class.
        /// </summary>
        /// <param name="session">The session this instance is operating on.</param>
        internal SynchronizationState(SessionContext session) : base(session)
        {

        }

        /// <inheritdoc/>
        internal override void Register()
        {
            Logger.Info("Registering SynchronizationState");
            session.State = SessionStates.Connected;
            session.ApplicationConnection.Received += ApplicationConnectionReceived;
        }

        /// <inheritdoc/>
        internal override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Handles the following event types: CloseSessionMessage, ChatMessage, 
        /// ExecutionResultMessage.
        /// </remarks>
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
