
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TwinEditor.UI.Settings
{
    public partial class SettingsDialog : Form
    {
        private ApplicationContext applicationContext;
        
        public SettingsDialog(ApplicationContext applicationContext)
        {
            InitializeComponent();

            this.applicationContext = applicationContext;

            InitializeSettings();
        }
        
        void InitializeSettings()
        {
            var appSettings = Configuration.ApplicationSettings.Default;            
            
            userName.Text = appSettings.UserName;
            
            string s = "Lebowski.Synchronization.DifferentialSynchronization.DifferentialSynchronizationStrategy";
            synchronizationStrategy.Items.Add(s);
            synchronizationStrategy.SelectedItem = s;            
        }
        
        void ApplyButtonClick(object sender, EventArgs e)
        {
            if(!Validate())
            {
                // TODO: invalid entries
            }
            
            var appSettings = Configuration.ApplicationSettings.Default;
            
        	// General settings
        	appSettings.UserName = userName.Text;
        	appSettings.SynchronizationStrategy = synchronizationStrategy.Text;
        	
        	appSettings.Save();
        }
        
        void CancelButtonClick(object sender, EventArgs e)
        {
            Hide();
            InitializeSettings();
        }
    }
}
