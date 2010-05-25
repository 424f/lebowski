
namespace TwinEditor.UI
{
    using System;
    using System.Windows.Forms;
    using System.Collections.Generic;
	/// <summary>
	/// Description of RecentFilesList.
	/// </summary>
	public class RecentFilesList
	{
		// TODO: shift to settings
		private int bufferSize;
		
		private List<string> recentFiles;
		
		public event EventHandler<RecentFilesChangedEventArgs> RecentFilesChanged;
		
		public RecentFilesList(int size)
		{
			recentFiles = new List<string>();
			bufferSize = size;
		}
		
		public RecentFilesList(int size, List<string> recentFiles)
		{
			this.recentFiles = recentFiles;
			bufferSize = size;
		}
		
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
		
		protected virtual void OnRecentFilesChanged(RecentFilesChangedEventArgs e)
		{
			if (RecentFilesChanged != null)
			{
				RecentFilesChanged(this, e);
			}
		}
	}
	
	public sealed class RecentFilesChangedEventArgs : EventArgs
    {
        public List<string> RecentFiles { get; private set; }
        
        public RecentFilesChangedEventArgs(List<string> recentFiles)
        {
            RecentFiles = recentFiles;
        }
    }
}
