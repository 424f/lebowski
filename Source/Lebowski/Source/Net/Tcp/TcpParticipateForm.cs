
namespace Lebowski.Net.Tcp
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    public sealed partial class TcpParticipateForm : Form
    {
        public event EventHandler<EventArgs> Submit;
        
        public string Address { get; private set; }
        public int Port { get; private set; }
        
        public TcpParticipateForm()
        {
            InitializeComponent();
            portText.Text = "1234";
            addressText.Text = "localhost";
        }
        
        void ParticipateClick(object sender, EventArgs e)
        {
            // Validate port
            int port;
            if (!int.TryParse(portText.Text, out port))
            {
                MessageBox.Show("Please enter a valid port number", "Invalid port");
                return;
            }
            
            Port = port;
            Address = addressText.Text;
            
            OnSubmit(new EventArgs());
        }
        
        void OnSubmit(EventArgs e)
        {
            if (Submit != null) {
                Submit(this, e);
            }
        }
        
        
        void TcpParticipateFormLoad(object sender, EventArgs e)
        {
            
        }
    }
}
