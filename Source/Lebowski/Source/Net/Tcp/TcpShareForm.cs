using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lebowski.Net.Tcp
{
    public sealed partial class TcpShareForm : Form
    {
        public int Port { get; private set; }
        
        public event EventHandler Submit;
        
        public TcpShareForm()
        {
            InitializeComponent();    
            portText.Text = "1234";
        }
        
        void ShareButtonClick(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(portText.Text, out port))
            {
                MessageBox.Show("Invalid port number", "Please enter a valid port number.");
                return;
            }
            Port = port;
            OnSubmit(new EventArgs());
        }
        
        void OnSubmit(EventArgs e)
        {
            if (Submit != null) {
                Submit(this, e);
            }
        }
        
    }
}
