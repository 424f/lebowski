namespace TwinEditor.UI
{
    using System;
    using TwinEditor.Sharing;
    
    /// <summary>
    /// Provides data for the <see cref="IApplicationView.CloseFile" /> event.
    /// </summary>
    public sealed class CloseFileEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CloseFileEventArgs.
        /// </summary>
        /// <param name="session">See <see cref="Session" />.</param>
        public CloseFileEventArgs(SessionContext session)
        {
            Session = session;
        }
        
        /// <summary>
        /// The session context which should be closed.
        /// </summary>
        public SessionContext Session { get; private set; }        
    }
}
