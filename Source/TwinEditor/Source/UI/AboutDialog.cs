

namespace TwinEditor.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    public partial class AboutDialog : Form
    {
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
