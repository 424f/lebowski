
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor.UI
{
    /// <summary>
    /// Form that shows an error message with optional additional information
    /// like the exception that caused the error
    /// </summary>
    public partial class ErrorMessage : Form
    {
        public event EventHandler<EventArgs> Report;
        
        public bool ShowReportButton
        {
            get { return reportButton.Visible; }
            set { reportButton.Visible = true; }
        }
        
        public static void Show(string title, string message, Exception e)
        {
            ErrorMessage form = new ErrorMessage(title, message, e);
            form.ShowDialog();
        }
        
        public ErrorMessage(string title, string message, Exception e)
        {
            InitializeComponent();
            
            titleLabel.Text = title;
            messageLabel.Text = message;            
            exceptionText.Text = e.ToString();
        }
        
        void ErrorMessageLoad(object sender, EventArgs e)
        {
            
        }
        
        void OkButtonClick(object sender, EventArgs e)
        {
            Close();
        }
        
        void ReportButtonClick(object sender, EventArgs e)
        {
            OnReport(new EventArgs());
            Close();
        }        
        
        protected virtual void OnReport(EventArgs e)
        {
            if (Report != null)
            {
                Report(this, e);
            }
        }
    }
}
