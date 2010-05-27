namespace TwinEditor.UI
{
    using System;    
    using TwinEditor.FileTypes;
    
    /// <summary>
    /// Provides data for the <see cref="IApplicationView.NewFile" /> event.
    /// </summary>
    public sealed class NewFileEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NewFileEventArgs class with
        /// the IFileType of the file that should be created.
        /// </summary>
        /// <param name="fileType">See <see cref="FileType" /></param>
        public NewFileEventArgs(IFileType fileType)
        {
            FileType = fileType;
        }
        
        /// <summary>
        /// The new file's type.
        /// </summary>
        public IFileType FileType { get; private set; }        
    }    
}
