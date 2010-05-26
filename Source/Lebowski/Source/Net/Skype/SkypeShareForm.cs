namespace Lebowski.Net.Skype
{
    using System;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    
    /// <summary>
    /// The form that is displayed to the user when he would like to share
    /// a skype session, providing the user with choice about the exact
    /// session parameters.
    /// </summary>
    public sealed partial class SkypeShareForm : Form
    {
        private SkypeProtocol protocol;        
        
        /// <summary>
        /// Initializes a new object of the SkypeShareForm class, which will
        /// utilize the provided SkypeProtocol to access the Skype API.
        /// </summary>
        /// <param name="protocol">The SkypeProtocol this form will utilize.</param>
        public SkypeShareForm(SkypeProtocol protocol)
        {
            InitializeComponent();

            this.protocol = protocol;

            ResourceManager rm = new ResourceManager(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly());
            Image image = (Image)rm.GetObject("SkypeUserImage");            
            
            foreach (string username in protocol.FriendNames)
            {
                if (protocol.IsUserOnline(username))
                {
                    dataGridView.Rows.Add(new object[]{ image, username });
                }
            }
        }

        /// <summary>
        /// Gets the user name of the currently selected friend
        /// </summary>
        public string SelectedUser
        {
            get
            {
                return (string)dataGridView.SelectedRows[0].Cells[1].Value;
            }
        }

        private void ShareButtonClick(object sender, EventArgs e)
        {
            OnSubmit(new EventArgs());
        }

        void CancelButtonClick(object sender, EventArgs e)
        {
            OnCancel(new EventArgs());
            Close();
        }
        
        void SkypeShareFormFormClosed(object sender, FormClosedEventArgs e)
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