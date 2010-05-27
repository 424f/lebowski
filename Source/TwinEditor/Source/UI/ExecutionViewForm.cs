namespace TwinEditor.UI
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using TwinEditor.Execution;

    /// <summary>
    /// A view that displays the data provided by an <see cref="ExecutionResult" />.
    /// </summary>
    public partial class ExecutionViewForm : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ExecutionViewForm with an 
        /// ExecutionResult.
        /// </summary>
        /// <param name="executionResult"></param>
        public ExecutionViewForm(ExecutionResult executionResult)
        {
            InitializeComponent();
            this.executionResult = executionResult;
            executionResult.ExecutionChanged += ExecutionResultChanged;
        }        
        
        /// <summary>
        /// The ExecutionResult whose events should be captured and displayed
        /// in this form.
        /// </summary>
        public ExecutionResult ExecutionResult
        {
            get
            {
                return executionResult;
            }

            set
            {
                StandardOutput.Invoke((Action)delegate { StandardOutput.Text = ""; });
                if (executionResult != null)
                {
                    executionResult.ExecutionChanged -= ExecutionResultChanged;
                }
                value.ExecutionChanged += ExecutionResultChanged;
                executionResult = value;
            }
        }
        private ExecutionResult executionResult;


        private void ExecutionTabControlLoad(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handler for the <see cref="TwinEditor.Execution.ExecutionResult.ExecutionChanged" /> event.
        /// </summary>
        /// <param name="o">The event issuer.</param>
        /// <param name="e">The event data.</param>
        private void ExecutionResultChanged(object o, ExecutionChangedEventArgs e)
        {
            StandardOutput.Invoke((Action) delegate
            {
                StandardOutput.Text += e.StandardOut;
            });
        }
    }
}