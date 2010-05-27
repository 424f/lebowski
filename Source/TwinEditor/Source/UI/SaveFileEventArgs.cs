namespace TwinEditor.UI
{
    using System;
    using TwinEditor.Sharing;
    
    /// <summary>
    /// Provides data for the <see cref="IApplicationView.SaveFile" /> event.
    /// </summary>
    public sealed class SaveFileEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the SaveFileEventArgs class with
        /// a session and the path where it should be stored.
        /// </summary>
        /// <param name="session">See <see cref="Session" />.</param>
        /// <param name="fileName">See <see cref="FileName" />.</param>
        public SaveFileEventArgs(SessionContext session, string fileName)
        {
            Session = session;
            FileName = fileName;
        }
        
        /// <summary>
        /// The Session to be saved.
        /// </summary>
        public SessionContext Session { get; private set; }
        
        /// <summary>
        /// The path where the contents of the session should be stored.
        /// </summary>
        public string FileName { get; private set; }        
    }
}
