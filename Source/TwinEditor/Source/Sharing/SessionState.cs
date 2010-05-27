namespace TwinEditor.Sharing
{
    using Lebowski.Net;
    using log4net;
    
    /// <summary>
    /// Describes the behavior of a <see cref="SessionContext">SessionContext</see>
    /// in a specific state.
    /// </summary>
    internal class SessionState
    {
        /// <summary>
        /// Initializes a new member of the SessionState class, associated
        /// with a provided <see cref="SessionContext">SessionContext</see>.
        /// </summary>
        /// <param name="session">The session this state will be operating on.</param>
        internal SessionState(SessionContext session)
        {
            this.session = session;
        }        
        
        /// <summary>
        /// Provides logging functionality.
        /// </summary>
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(SessionState));
        
        /// <summary>
        /// The session this state is operating on.
        /// </summary>
        protected SessionContext session;

        /// <summary>
        /// Registers this state with the session's events.
        /// </summary>
        internal virtual void Register()
        {

        }

        /// <summary>
        /// Unregisters this state from the session's events.
        /// </summary>        
        internal virtual void Unregister()
        {

        }

        /// <summary>
        /// Handles messages received from the session's application connection.
        /// </summary>
        /// <param name="sender">The issuer of the event.</param>
        /// <param name="e">The event data, describing the packet that was received.</param>
        protected virtual void ApplicationConnectionReceived(object sender, ReceivedEventArgs e)
        {

        }
    }
}
