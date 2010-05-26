namespace Lebowski.Net.Tcp
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// The form that is displayed when the user would like to share
    /// a session using TCP. It provides the user with choice about the exact
    /// connection settings.
    /// </summary>       
    public sealed partial class TcpShareForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the TcpShareForm class.
        /// </summary>
        public TcpShareForm()
        {
            InitializeComponent();
            portText.Text = "1234";
        }

        /// <summary>
        /// The TCP port that has been chosen.
        /// </summary>
        public int Port { get; private set; }        
        
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
        
        void TcpShareFormFormClosed(object sender, FormClosedEventArgs e)
        {
            OnCancel(new EventArgs());        	
        }        
        
        void CancelButtonClick(object sender, EventArgs e)
        {
            OnCancel(new EventArgs());
            Close();
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