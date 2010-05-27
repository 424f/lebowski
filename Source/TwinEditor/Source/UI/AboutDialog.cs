namespace TwinEditor.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// The AboutDialog displays information about the application to the user.
    /// </summary>
    public partial class AboutDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the AboutDialog class.
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
        }

        void UrlLabelClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlLabel.Text);
        }
    }
}