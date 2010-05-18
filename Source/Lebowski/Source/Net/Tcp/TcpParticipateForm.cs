using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lebowski.Net.Tcp
{
	public sealed partial class TcpParticipateForm : Form
	{
		public event EventHandler<EventArgs> Submit;
		
		public string Address { get; private set; }
		public int Port { get; private set; }
		
		public TcpParticipateForm()
		{
			InitializeComponent();
		}
		
		void ParticipateClick(object sender, EventArgs e)
		{
			// Validate IPv4 address
			if (!NetUtils.IsValidIP(addressText.Text))
			{
				MessageBox.Show("Please enter valid IPv4 address", "Invalid IPv4 address");
				return;
			}
			// Validate port
			int port;
			if(!int.TryParse(portText.Text, out port))
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
		
	}
}
