namespace TwinEditor.Sharing
{
    using Lebowski.Net;
    using Lebowski.Synchronization;
    
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
}
