
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
        public static void Show(string title, string message, Exception e)
        {
            ErrorMessage form = new ErrorMessage();
            form.ShowDialog();
        }
        
        private ErrorMessage()
        {
            InitializeComponent();
        }
        
        void ErrorMessageLoad(object sender, EventArgs e)
        {
        	
        }
    }
}
