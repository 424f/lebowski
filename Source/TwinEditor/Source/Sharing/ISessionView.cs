namespace TwinEditor.Sharing
{
    using System;
    using Lebowski;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;
    using Lebowski.TextModel;
    using TwinEditor.FileTypes;
    using log4net;
    
    /// <summary>
    /// A view that displays the data of a session.
    /// </summary>
    public interface ISessionView
    {
        /// <summary>
        /// Gets the session context representing the behavior and data for this view.
        /// </summary>
        SessionContext SessionContext { get; }
        
        /// <summary>
        /// Gets and sets the OnDisk property.
        /// </summary>
        /// <value>
        /// Indicates whether the session's document corresponds to a file
        /// on the disk.
        /// </value>
        bool OnDisk { get; set; }
        
        /// <summary>
        /// Gets and sets the FileModified property
        /// </summary>
        /// <value>
        /// Indicates whether the session's document has been changed since
        /// its last save.
        /// </value>
        bool FileModified { get; set; }
    }
}