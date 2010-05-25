using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor.UI
{
    public partial class ExecutionViewForm : UserControl
    {        
        public ExecutionResult ExecutionResult
        {
            get
            {
                return executionResult;    
            }
            
            set
            {
                StandardOutput.Invoke((Action)delegate { StandardOutput.Text = ""; });
                if(executionResult != null)
                {
                    executionResult.ExecutionChanged -= ExecutionResultChanged;
                }
                value.ExecutionChanged += ExecutionResultChanged;
                executionResult = value;
            }
        }
        private ExecutionResult executionResult;
        
        public ExecutionViewForm(ExecutionResult executionResult)
        {
            InitializeComponent();
            
            this.executionResult = executionResult;
            
            executionResult.ExecutionChanged += ExecutionResultChanged;
        }
        
        void ExecutionTabControlLoad(object sender, EventArgs e)
        {
            
        }
        
        public void ExecutionResultChanged(object o, ExecutionChangedEventArgs e)
        {
            StandardOutput.Invoke((Action) delegate
            {
                StandardOutput.Text += e.StandardOut;
            });
        }
    }
}
