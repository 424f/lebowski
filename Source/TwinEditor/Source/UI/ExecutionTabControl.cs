using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor.UI
{
	public partial class ExecutionTabControl : UserControl
	{
		public void SetStandardOutput(string text)
		{
			StandardOutput.Text = text;
		}
		
		public ExecutionTabControl()
		{
			InitializeComponent();
		}
		
		void ExecutionTabControlLoad(object sender, EventArgs e)
		{
			
		}
	}
}
