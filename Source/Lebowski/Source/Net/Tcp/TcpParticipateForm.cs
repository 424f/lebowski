namespace Lebowski.Net.Tcp
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// The form that is displayed to the user when he would like to participate
    /// in a session using TCP. It provides the user with choice about the exact
    /// connection settings.
    /// </summary>    
    public sealed partial class TcpParticipateForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the TcpParticipateForm.
        /// </summary>
        public TcpParticipateForm()
        {
            InitializeComponent();
            portText.Text = "1234";
            addressText.Text = "localhost";
        }

        /// <summary>
        /// Gets the IP address or hostname that has been entered.
        /// </summary>
        public string Address { get; private set; }
        
        /// <summary>
        /// Gets the TCP port that has been entered.
        /// </summary>
        public int Port { get; private set; }
        
        
        private void ParticipateClick(object sender, EventArgs e)
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

        void CancelButtonClick(object sender, EventArgs e)
        {
            OnCancel(new EventArgs());
            Close();
        }        
        
        void TcpParticipateFormFormClosed(object sender, FormClosedEventArgs e)
        {
        	OnCancel(new EventArgs());
        }        
        
        /// <summary>
        /// Raises the Submit event
        /// </summary>
        private void OnSubmit(EventArgs e)
        {
            if (Submit != null)
            {
                Submit(this, e);
            }
        }
        
        /// <summary>
        /// Raises the Cancel event
        /// </summary>
        private void OnCancel(EventArgs e)
        {
            if (Cancel != null)
            {
                Cancel(this, e);
            }
        }        
        
        /// <summary>
        /// Occurs when the user submits the form
        /// </summary>
        public event EventHandler Submit;        
        
        /// <summary>
        /// Occurs when the user cancels the dialog
        /// </summary>        
        public event EventHandler Cancel;     
    }
}