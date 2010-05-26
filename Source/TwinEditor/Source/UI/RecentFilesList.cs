
namespace TwinEditor.UI
{
    using System;
    using System.Windows.Forms;
    using System.Collections.Generic;
    /// <summary>
    /// RecentFilesList is a bounded list that realized a most recently used list (MRUList)
    /// </summary>
    public class RecentFilesList
    {
        private int bufferSize;
        private List<string> recentFiles;
        /// <summary>
        /// Occurs when recent files list has changed.
        /// </summary>
        public event EventHandler<RecentFilesChangedEventArgs> RecentFilesChanged;

        /// <summary>
        /// Constructor. 
        /// Initializies a new instance of RcentFilesList
        /// </summary>
        /// <param name="size">Max size of the list</param>
        public RecentFilesList(int size)
        {
            recentFiles = new List<string>();
            bufferSize = size;
        }

        /// <summary>
        /// CopyConstructor.
        /// Initializes a new instance of RecentFilesList
        /// </summary>
        /// <param name="size">Max size of the list</param>
        /// <param name="recentFiles">The list to copy from</param>
        public RecentFilesList(int size, List<string> recentFiles)
        {
            this.recentFiles = recentFiles;
            bufferSize = size;
        }
        
        /// <summary>
        /// Max size of the recent files list.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                bufferSize = Math.Max(value, 0);
            }
        }

        /// <summary>
        /// Adds the given filename to the recent files list.
        /// Removes previous occurence if necessary and place new filename at the beginning of the list.
        /// </summary>
        /// <param name="filename">The filename of the file to be added as recent file</param>
        public void Add(string filename)
        {
            // remove file from list (if already in)
            recentFiles.Remove(filename);
            // if recent files list is full, remove the oldest entry
            if (recentFiles.Count == bufferSize)
                recentFiles.RemoveAt(bufferSize - 1);
            // insert the newly added filename
            recentFiles.Insert(0, filename);
            OnRecentFilesChanged(new RecentFilesChangedEventArgs(recentFiles));
        }

        /// <summary>
        /// Fires the RecentFilesChanged event.
        /// Is called when a new filename has been added to the recent files list.
        /// </summary>
        /// <param name="e">A <see cref="RecentFilesChangedEventArgs"></see> that contains the event data</param>
        protected virtual void OnRecentFilesChanged(RecentFilesChangedEventArgs e)
        {
            if (RecentFilesChanged != null)
            {
                RecentFilesChanged(this, e);
            }
        }
    }

    /// <remarks>
    /// <see cref="RecentFilesChangedEventArgs"></see> is the EventArgs class for <see cref="RecentFilesChanged"></see>
    /// </remarks>
    public sealed class RecentFilesChangedEventArgs : EventArgs
    {
        public List<string> RecentFiles { get; private set; }

        public RecentFilesChangedEventArgs(List<string> recentFiles)
        {
            RecentFiles = recentFiles;
        }
    }
}