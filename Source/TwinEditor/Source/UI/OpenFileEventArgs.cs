using System;

namespace TwinEditor.UI
{
    /// <summary>
    /// Provides data for the <see cref="IApplicationView.OpenFile" /> event.
    /// </summary>
    public sealed class OpenFileEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the OpenFileEventArgs class with
        /// a fileName to be opened.
        /// </summary>
        /// <param name="fileName">The path to the file to be opened.</param>
        public OpenFileEventArgs(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// The file to be opened.
        /// </summary>
        public string FileName { get; private set; }
        
    }
}
