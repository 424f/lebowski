using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lebowski.UI.FileTypes;

namespace TwinEditor
{
	public partial class FileTabControl : UserControl
	{
		public string FileName { get; set; }
		public IFileType FileType { get; set; }
		
		public FileTabControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
