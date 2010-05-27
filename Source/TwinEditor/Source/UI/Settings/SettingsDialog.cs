namespace TwinEditor.UI.Settings
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// Displays the application settings and allows the user to make changes to them.
    /// </summary>
    public partial class SettingsDialog : Form
    {
        private TwinEditor.ApplicationContext applicationContext;

        /// <summary>
        /// Initializes a new instance of the SettingsDialog, based on a
        /// specified application context.
        /// </summary>
        /// <param name="applicationContext">The specified application context.</param>
        public SettingsDialog(TwinEditor.ApplicationContext applicationContext)
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
            if (!Validate())
            {
                // TODO: invalid entries
            }

            var appSettings = Configuration.ApplicationSettings.Default;

            // General settings
            appSettings.UserName = userName.Text;
            appSettings.SynchronizationStrategy = synchronizationStrategy.Text;

            appSettings.Save();
            Close();
        }

        void CancelButtonClick(object sender, EventArgs e)
        {
            Hide();
            InitializeSettings();
        }
    }
}