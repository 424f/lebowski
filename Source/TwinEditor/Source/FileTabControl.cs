using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.TextModel;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.UI.FileTypes;

namespace TwinEditor
{
	public partial class FileTabControl : UserControl, ISessionContext
	{
		public string FileName { get; set; }
		public bool OnDisk { get; set; }
		private IFileType fileType;
		public IFileType FileType
		{
			get { return fileType; }
			set
			{
				fileType = value;
				// TODO: build new .dll to include python syntax highlight definition (.xshd)
				// SourceCode.SetHighlighting(fileType.Name);
				SourceCode.SetHighlighting("Boo");
			}
		}
		public int NumExecutions { get; protected set; }
		public void IncrementNumExections()
		{
			NumExecutions += 1;
		}
		
		public ITextContext Context { get; protected set; }
		
		public IConnection Connection { get; protected set; }
		public IConnection ApplicationConnection { get; protected set; }
		public DifferentialSynchronizationStrategy SynchronizationStrategy { get; protected set; }
	
		public FileTabControl()
		{
			this.OnDisk = false;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			// Create a text context for the source code editor
			Context = new TextEditorTextContext(SourceCode);
		}
		
		public void Close()
		{
			
		}
		
		public void StartSession(Lebowski.Synchronization.DifferentialSynchronization.DifferentialSynchronizationStrategy strategy, Lebowski.Net.IConnection connection, IConnection applicationConnection)
		{
			SynchronizationStrategy = strategy;
			Connection = connection;
			ApplicationConnection = applicationConnection;
		}
		
		public void CloseSession()
		{
			
		}
		
	}
}
