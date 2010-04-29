using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski.TextModel;
using Lebowski.UI.FileTypes;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace TwinEditor
{
	public partial class FileTabControl : UserControl
	{
		public string FileName { get; set; }
		public IFileType FileType { get; set; }
		public ITextContext Context { get; protected set; }
		
		public IConnection Connection { get; protected set; }
		public DifferentialSynchronizationStrategy SynchronizationStrategy { get; protected set; }
	
		public FileTabControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			// Create a text context for the source code editor
			Context = new TextEditorTextContext(SourceCode);
		}
	}
}
