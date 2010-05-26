namespace TwinEditor.Sharing
{
    using System;
    using Lebowski;
    using Lebowski.Net;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using TwinEditor.Sharing;
    
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
            session.ApplicationConnection.Send("HI 0");
            Logger.Info("Sending 'HI'");
        }

        public override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }

        protected override void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {
            if ((string)e.Message == "HI 0")
            {
                Console.WriteLine(session.SiteId.ToString() + ": " + e.Message);
                session.ApplicationConnection.Send("HI 1");
            }
            else if ((string)e.Message == "HI 1")
            {
                Console.WriteLine(session.SiteId.ToString() + ": " + e.Message);
                session.ApplicationConnection.Send("HI 1");
                session.ActivateState(new InitializationState(session));
            }
            else
            {
                base.ApplicationConnectionReceived(sender, e);
            }

        }
    }
}
