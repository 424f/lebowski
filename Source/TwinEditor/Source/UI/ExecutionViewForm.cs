using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor.UI
{
	public partial class ExecutionViewForm : UserControl
	{		
	    private ExecutionResult executionResult;
	    
		public ExecutionViewForm(ExecutionResult executionResult)
		{
			InitializeComponent();
			
			this.executionResult = executionResult;
			
			executionResult.ExecutionChanged += delegate(object sender, ExecutionChangedEventArgs e)
			{
			    StandardOutput.Invoke((Action) delegate
                {
	    		    StandardOutput.Text += e.StandardOut;
			    });
			};
		}
		
		void ExecutionTabControlLoad(object sender, EventArgs e)
		{
			
		}
	}
}
