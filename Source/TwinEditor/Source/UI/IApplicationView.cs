using System.Collections.Generic;

namespace TwinEditor.UI
{
    using System;    
    using Lebowski;
    using Lebowski.Net;
    using TwinEditor.FileTypes;
    using TwinEditor.Sharing;
    
    /// <summary>
    /// Provides a view for the top-level application information, provided by the
    /// ApplicationContext.
    /// </summary>
    public interface IApplicationView
    {
        /// <summary>
        /// The ApplicationContext that provides data and behavior for this view.
        /// </summary>        
        ApplicationContext ApplicationContext { get; set; }        
        
        /// <summary>
        /// Gets or sets the <see cref="IFileType" />s that are available 
        /// in this application.
        /// </summary>
        /// <remarks>
        /// This should be set by the <see cref="ApplicationContext" /> object.
        /// </remarks>        
        IFileType[] FileTypes { get; set; }
        
        /// <summary>
        /// Sets or gets the communication protocols that this application supports.
        /// </summary>
        /// <remarks>
        /// This should be set by the <see cref="ApplicationContext" /> object.
        /// </remarks>        
        ICommunicationProtocol[] CommunicationProtocols { get; set; }

        /// <summary>
        /// Creates a new session view using the provided file type.
        /// </summary>
        /// <param name="fileType">The file type to be initially used for this session.</param>
        /// <returns></returns>
        ISessionView CreateNewSession(IFileType fileType);

        /// <summary>
        /// Display an error with the given message and depending
        /// on application settings possibly with the exception provided to the user.
        /// </summary>
        /// <param name="message">The text message, giving a user-friendly description of the error.</param>
        /// <param name="exception">The exception that caused the error, giving
        /// more insight to developers about what exactly happened.</param>        
        void DisplayError(string message, Exception exception);        

        /// <summary>
        /// Makes sure the view is currently visible to the user.
        /// </summary>
        void Show();        
        
        /// <summary>
        /// Used to update this view when the rcently used files have changed.
        /// </summary>
        /// <param name="recentFiles">The recent files opened by the user.</param>        
        void UpdateRecentFiles(List<String> recentFiles);

        /// <summary>
        /// Initiated when the user wants to close the application.
        /// </summary>
        event EventHandler<EventArgs> ApplicationClosing;        
        
        /// <summary>
        /// Occurs when the user initiated a CloseFile operation.
        /// </summary>
        event EventHandler<CloseFileEventArgs> CloseFile;

        /// <summary>
        /// Occurs when the user initiates the creation of a new file.
        /// </summary>
        event EventHandler<NewFileEventArgs> NewFile;        
        
        /// <summary>
        /// Occurs when the user initiated an OpenFile operation.
        /// </summary>
        event EventHandler<OpenFileEventArgs> OpenFile;
        
        /// <summary>
        /// Occurs when the user wants to participate in a shared session.
        /// </summary>
        event EventHandler<ParticipateEventArgs> Participate;        
        
        /// <summary>
        /// Occurs when the user initiated a SaveFile operation.
        /// </summary>
        event EventHandler<SaveFileEventArgs> SaveFile;   
        
        /// <summary>
        /// Occurs when the user initiated a shared session.
        /// </summary>
        event EventHandler<ShareSessionEventArgs> ShareSession;
    } 
}