namespace TwinEditor.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// Form that shows an error message with optional additional information
    /// like the exception that caused the error
    /// </summary>
    public partial class ErrorMessage : Form
    {
        /// <summary>
        /// Initializes a new instance of the ErrorMessage class with 
        /// an error title, an error message and the exception that was
        /// cause for the error.
        /// </summary>
        /// <param name="title">The title for this form.</param>
        /// <param name="message">The message that should be displayed to the user.</param>
        /// <param name="e">The exception that was cause for the error.</param>
        public ErrorMessage(string title, string message, Exception e)
        {
            InitializeComponent();

            titleLabel.Text = title;
            messageLabel.Text = message;
            exceptionText.Text = e.ToString();
        }        
        
        /// <summary>
        /// Sets or gets the ShowReport property.
        /// </summary>
        /// <value>
        /// Determines whether the form should display a 'Report' button to the user.
        /// </value>
        public bool ShowReportButton
        {
            get { return reportButton.Visible; }
            set { reportButton.Visible = true; }
        }

        /// <summary>
        /// Displays a new ErrorMessage and blocks until it is closed
        /// </summary>
        /// <param name="title">The title for the form to be created.</param>
        /// <param name="message">The message to be displayed to the user.</param>
        /// <param name="e">The exception that caused the error.</param>
        public static void Show(string title, string message, Exception e)
        {
            ErrorMessage form = new ErrorMessage(title, message, e);
            form.ShowDialog();
        }

        private void ErrorMessageLoad(object sender, EventArgs e)
        {

        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ReportButtonClick(object sender, EventArgs e)
        {
            OnReport(new EventArgs());
            Close();
        }

        /// <summary>
        /// Raies the <see cref="Report" /> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnReport(EventArgs e)
        {
            if (Report != null)
            {
                Report(this, e);
            }
        }
        
        /// <summary>
        /// Occurs when the user selects to report the error.
        /// </summary>
        public event EventHandler<EventArgs> Report;
    }
}