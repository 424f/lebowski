namespace TwinEditor.Sharing
{
    using Lebowski.Net;
    using log4net;
    
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
}
