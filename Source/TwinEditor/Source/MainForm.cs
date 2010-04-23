
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			SourceCode.SetHighlighting("C#");
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
