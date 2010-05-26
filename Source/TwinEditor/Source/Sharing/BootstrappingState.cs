namespace TwinEditor.Sharing
{
    using System;
    using Lebowski;
    using Lebowski.Net;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using TwinEditor.Sharing;
    
    /// <summary>
    /// Will bootstrap a session. This currently does not do much besides
    /// screaming "HI" at each other, but it might be used in the future to:
    ///     - Make sure application versions are compatible
    ///     - Agree on a synchronization strategy
    ///     - Check password information
    /// </summary>
    public class BootstrappingState : SessionState
    {
        /// <summary>
        /// Initializes a new instance of the BootstrappingState class.
        /// </summary>
        /// <param name="session">The session this instance will be operating on.</param>
        public BootstrappingState(SessionContext session) : base(session) {}

        /// <inheritdoc/>
        public override void Register()
        {
            Logger.Info("Registering BootstrappingState");

            session.State = SessionStates.Connecting;
            session.ApplicationConnection.Received += ApplicationConnectionReceived;

            // If we are the client, we'll say hi to the client
            session.ApplicationConnection.Send("HI 0");
            Logger.Info("Sending 'HI'");
        }

        /// <inheritdoc/>
        public override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Handles the following events:
        /// <list type="bullet">
        ///     <item>
        ///         <term>"HI 0"</term>
        ///         <description>Initial handshake</description>
        ///     </item>
        /// 
        ///     <item>
        ///         <term>"HI 1"</term>
        ///         <description>Second handshake. Afer receiving this, we will change to 
        ///         the <see cref="InitializationState">Initializationtate</see>.</description>
        ///     </item>       
        /// </list>
        /// </remarks>
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
