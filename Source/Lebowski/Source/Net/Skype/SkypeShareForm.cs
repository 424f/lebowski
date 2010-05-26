

namespace Lebowski.Net.Skype
{
    using System;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    
    public partial class SkypeShareForm : Form
    {
        public event EventHandler Submit;

        private SkypeProtocol protocol;

        public string SelectedUser
        {
            get
            {
                return (string)dataGridView.SelectedRows[0].Cells[1].Value;
            }
        }

        public SkypeShareForm(SkypeProtocol protocol)
        {
            InitializeComponent();

            this.protocol = protocol;

            ResourceManager rm = new ResourceManager(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly());
            Image image = (System.Drawing.Image)rm.GetObject("SkypeUserImage");            
            
            foreach (string username in protocol.FriendNames)
            {
                if (protocol.IsUserOnline(username))
                {
                    dataGridView.Rows.Add(new object[]{ image, username });
                }
            }
        }

        void SkypeShareFormLoad(object sender, EventArgs e)
        {

        }

        void ShareButtonClick(object sender, EventArgs e)
        {
            OnSubmit(new EventArgs());
        }

        protected virtual void OnSubmit(EventArgs e)
        {
            if (Submit != null) {
                Submit(this, e);
            }
        }

    }
}