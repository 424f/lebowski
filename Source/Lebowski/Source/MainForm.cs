using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace LebowskiMockups
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			
			var editorControl = new TextEditorControl();
			editorControl.Dock = DockStyle.Fill;
			editorControl.ConvertTabsToSpaces = true;
			editorControl.ShowSpaces = true;
			editorControl.SetHighlighting("C#");
			//editorControl.Document.FormattingStrategy = new ICSharpCode.PythonBinding.PythonFormattingStrategy();
			CodePanel.Controls.Add(editorControl);
			
			var images = new ImageList();
			//images.Images.Add(Image.FromFile("C:/users/bo/desktop/icon_python.png"));
			tabControl1.ImageList = images;
		}
		
		void RichTextBox1TextChanged(object sender, EventArgs e)
		{
		
		}
		
		void CodePanelPaint(object sender, PaintEventArgs e)
		{
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void TabPage4Click(object sender, EventArgs e)
		{
			
		}
	}
}
