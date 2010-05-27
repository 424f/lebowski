namespace TwinEditor.UI
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Resources;
    using System.Reflection;
    using Lebowski;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;
    using TwinEditor.FileTypes;
    using TwinEditor.UI;
    using TwinEditor.Configuration;
    using TwinEditor.Sharing;
    using log4net;

    /// <summary>
    /// Displalys the information in a ApplicationContext using a WinForms
    /// user interface.
    /// </summary>
    public partial class ApplicationViewForm : Form, IApplicationView
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationViewForm));
        
        private Settings.SettingsDialog SettingsDialog;
        
        private ApplicationSettings appSettings = Configuration.ApplicationSettings.Default;
        
        /// <summary>
        /// The number that is assigned to next new file without name
        /// </summary>
        private int nextFileNumber = 1;

        /// <summary>
        /// Stores the session views that are currently being displayed
        /// </summary>
        private List<SessionViewForm> sessionViews = new List<SessionViewForm>();
        
        /// <summary>
        /// Stores the tab pages (containing <see cref="SessionViewForm" />s) that have been opened
        /// </summary>
        private List<TabPage> tabPages = new List<TabPage>();
        
        /// <summary>
        /// Stores the menu items for selection of the current file type.
        /// </summary>
        protected List<ToolStripMenuItem> chooseFileTypeMenuItems = new List<ToolStripMenuItem>();
            
        /// <summary>
        /// Initializes a new instance of the ApplicationViewForm.
        /// </summary>
        public ApplicationViewForm()
        {
            InitializeComponent();

            Logger.Info("MainForm component initialized.");

            // Clear tabs
            MainTab.TabPages.Clear();
            UpdateMenuItems();

            // Translate the menu
            TranslationUtil.TranslateMenuStrip(menuStrip1, ApplicationUtil.LanguageResources);

            // Configure settings dialog
            SettingsDialog = new Settings.SettingsDialog(ApplicationContext);

            // Get default settings
            var appSettings = Configuration.ApplicationSettings.Default;

            // Fire event when application is closing
            this.Closing += delegate { this.OnApplicationClosing(new EventArgs()); };
            
            // Handle closing a session view
            MainTab.TabClosed += delegate(object sender, TabClosedEventArgs e)
            {
                CloseTab(e.TabIndex);
            };
        }        

        /// <inheritdoc/>
        public IFileType[] FileTypes
        {
            get
            {
                return fileTypes;
            }
            set
            {
                if (FileTypes != null)
                    throw new NotImplementedException("Reseting file types is currently not supported.");
                fileTypes = value;

                openFileDialog.Filter = "";
                foreach (IFileType fileType in fileTypes)
                {
                    var menuItem = new ToolStripMenuItem(fileType.Name + " (" + fileType.FileNamePattern + ")");
                    IFileType currentFileType = fileType;
                    menuItem.Click += delegate
                    {
                        OnNewFile(new NewFileEventArgs(currentFileType));
                    };
                    newToolStripMenuItem.DropDown.Items.Add(menuItem);

                    // Add file type to open file dialog
                    string newFilter = string.Format("{0}|{1}", fileType.Name, fileType.FileNamePattern);
                    if (openFileDialog.Filter.Length > 0)
                    {
                        newFilter = "|" + newFilter;
                    }
                    openFileDialog.Filter += newFilter;

                    // Add file type as choice in 'Edit' menu
                    var chooseMenuItem = new ToolStripMenuItem(fileType.Name);
                    var editItems = editToolStripMenuItem.DropDown.Items;
                    chooseMenuItem.Tag = fileType;
                    chooseMenuItem.Enabled = false;
                    editItems.Insert(editItems.Count-1, chooseMenuItem);
                    chooseMenuItem.Click += delegate
                    {
                        sessionViews[MainTab.SelectedIndex].SessionContext.FileType = (IFileType)chooseMenuItem.Tag;
                        UpdateMenuItems();
                    };
                    chooseFileTypeMenuItems.Add(chooseMenuItem);
                }

                // Add a separator after the file type choices
                editToolStripMenuItem.DropDown.Items.Insert(editToolStripMenuItem.DropDown.Items.Count-1, new ToolStripSeparator());

                // Load icons for tab control for different file types
                ImageList imageList = new ImageList();
                foreach (IFileType fileType in FileTypes)
                {
                    try
                    {
                        imageList.Images.Add(fileType.Name + "Image", fileType.Icon);

                    }
                    catch (Exception e)
                    {
                        Logger.WarnFormat("Could not load image for type {0}: {1}", fileType.Name, e);
                    }
                }
                MainTab.ImageList = imageList;
            }
        }
        private IFileType[] fileTypes;

        /// <inheritdoc/>
        public ICommunicationProtocol[] CommunicationProtocols
        {
            get
            {
                return communicationProtocols;
            }

            set
            {
                if (communicationProtocols != null)
                {
                    throw new NotImplementedException("Reseting of communication protocols currently not supported.");
                }
                communicationProtocols = value;

                foreach (ICommunicationProtocol protocol in communicationProtocols)
                {
                    ICommunicationProtocol currentProtocol = protocol;

                    // After initializing, if the protocol is not enabled, we discard it
                    if (!protocol.Enabled)
                        continue;

                    // Add menu item to share session
                    if (protocol.CanShare)
                    {
                        var menuItem = new ToolStripMenuItem(protocol.Name);
                        menuItem.Click += delegate
                        {
                            OnShareSession(new ShareSessionEventArgs(sessionViews[MainTab.SelectedIndex].SessionContext, currentProtocol));
                        };
                        shareToolStripMenuItem.DropDown.Items.Add(menuItem);
                    }

                    // Add menu item to join session
                    if (protocol.CanParticipate)
                    {
                        var menuItem = new ToolStripMenuItem(protocol.Name);
                        menuItem.Click += delegate
                        {
                            OnParticipate(new ParticipateEventArgs(sessionViews[MainTab.SelectedIndex].SessionContext, currentProtocol));
                        };
                        participateToolStripMenuItem.DropDown.Items.Add(menuItem);
                    }
                }
            }
        }
        private ICommunicationProtocol[] communicationProtocols;

        /// <inheritdoc/>
        public TwinEditor.ApplicationContext ApplicationContext { get; set; }        
        
        /// <inheritdoc/>
        /// <remarks>Display an error dialog.</remarks>
        public void DisplayError(string message, System.Exception exception)
        {
            ErrorMessage errorMessage = new ErrorMessage("An error occurred", message, exception);
            errorMessage.ShowDialog();
        }
        
        /// <summary>
        /// Updates GUI elements after the program's state has changed
        /// </summary>
        public void UpdateGuiState()
        {
            UpdateMenuItems();
        }

        /// <summary>
        /// Updates the menu items, making sure that only menu items that 
        /// are possible to use in a current state are enabled.
        /// </summary>
        private void UpdateMenuItems()
        {
            bool fileOpen = MainTab.TabPages.Count > 0;
            
            shareToolStripMenuItem.Enabled = fileOpen;

            foreach (ToolStripMenuItem item in chooseFileTypeMenuItems)
            {
                item.Enabled = fileOpen;
                item.Checked = MainTab.SelectedIndex != -1 && item.Tag == sessionViews[MainTab.SelectedIndex].SessionContext.FileType;
            }

            if (MainTab.TabPages.Count == 0)
            {
                scriptToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveAllToolStripMenuItem.Enabled = false;
                printToolStripMenuItem.Enabled = false;
            }
            else
            {
                closeToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveAllToolStripMenuItem.Enabled = true;
                scriptToolStripMenuItem.Enabled = true;
                printToolStripMenuItem.Enabled = true;
                var tab = sessionViews[MainTab.SelectedIndex];
                compileToolStripMenuItem.Enabled = tab.SessionContext.FileType.CanCompile;
                runToolStripMenuItem.Enabled = tab.SessionContext.FileType.CanExecute;
                shareToolStripMenuItem.Enabled = tab.SessionContext.State == SessionStates.Disconnected;
            }
        }

        /// <inheritdoc/>
        public void UpdateRecentFiles(List<string> recentFiles)
        {
        	if (recentFiles != null)
        	{
        		recentFilesToolStripMenuItem.Enabled = true;
	        	menuStrip1.Invoke((Action) delegate
                {
    		      	recentFilesToolStripMenuItem.DropDownItems.Clear();
    		      	
    				foreach (string file in recentFiles)
    				{
    					string filename = file; 
    					ToolStripMenuItem item = new ToolStripMenuItem(filename);
    					item.Click += delegate { OpenFile(this, new OpenFileEventArgs(filename)); };
    					recentFilesToolStripMenuItem.DropDownItems.Add(item);
    		      	}
                });
	        	appSettings.RecentFileList = recentFiles;
	        	appSettings.Save();
        	}
        	else
        	{
        		recentFilesToolStripMenuItem.Enabled = false;
        	}
        	
        }

        /// <inheritdoc/>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public ISessionView CreateNewSession(IFileType fileType)
        {
            return CreateNewTab(fileType);
        }

        private SessionViewForm CreateNewTab(IFileType fileType)
        {
            Logger.Info(string.Format("Creating new tab of file type {0}", fileType.Name));

            // Create a new tab and add a FileTabControl to it
            string filename = "New " + nextFileNumber++ + fileType.FileExtension;
            TabPage tabPage = new TabPage(filename);
            SessionViewForm tab = new SessionViewForm(this, tabPage);
            tabPage.Controls.Add(tab);
            sessionViews.Add(tab);
            tabPages.Add(tabPage);
            MainTab.TabPages.Add(tabPage);

            tab.Dock = DockStyle.Fill;
            tab.SessionContext.FileType = fileType;
            tab.SessionContext.FileName = filename;
            // Add callback for StateChanged in order to disable share menu item, when already shared
            tab.StateChanged += delegate(object sender, StateChangedEventArgs e)
            {
                if (e.State != SessionStates.Disconnected)
                {
                    this.shareToolStripMenuItem.Enabled = false;
                }
            };
            MainTab.SelectedTab = tabPage;
            UpdateMenuItems();
            return tab;
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            openFileDialog.RestoreDirectory = true;
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            OnOpen(new OpenFileEventArgs(openFileDialog.FileName));        
            UpdateMenuItems();
        }

        private void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
            sessionViews[MainTab.SelectedIndex].SourceCode.PrintDocument.Print();
        }

        private void MainTabSelected(object sender, TabControlEventArgs e)
        {
            /* After having selected a new tab, we have to update the
            menus to reflect the actions that are possible */
            UpdateMenuItems();
        }

        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            SessionViewForm tabControl = sessionViews[MainTab.SelectedIndex];
            SaveRequest(tabControl);
        }

        private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SessionViewForm tabControl = sessionViews[MainTab.SelectedIndex];
            SaveWithDialog(tabControl);
        }

        /// <summary>
        /// Checks whether the session is already on disk, and if yes calls 'Save' otherwise calls 'SaveWithDialog'.
        /// </summary>
        internal void SaveRequest(SessionViewForm sessionViewForm)
        {
            if (!sessionViewForm.OnDisk)
            {
                SaveWithDialog(sessionViewForm);
            }
            else
            {
                OnSave(new SaveFileEventArgs(sessionViewForm.SessionContext, sessionViewForm.SessionContext.FileName));
                sessionViewForm.OnDisk = true;
                sessionViewForm.FileModified = false;
                UpdateMenuItems();

            }
        }

        // opens saveFileDialog
        void SaveWithDialog(SessionViewForm tabControl)
        {
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = System.IO.Path.GetFileName(tabControl.SessionContext.FileName);
            saveFileDialog.Filter = string.Format("{0}|{1}", tabControl.SessionContext.FileType.Name, tabControl.SessionContext.FileType.FileNamePattern);
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tabControl.SessionContext.FileName = saveFileDialog.FileName;
                OnSave(new SaveFileEventArgs(tabControl.SessionContext, tabControl.SessionContext.FileName));
                tabControl.OnDisk = true;
                tabControl.FileModified = false;
            }
            else
            {
                Logger.Info(string.Format("User cancelled saving action {0}", saveFileDialog.FileName));
            }
        }

        void SaveAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (SessionViewForm tabControl in sessionViews)
            {
                SaveRequest(tabControl);
            }
        }

        void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        void CloseToolStripMenuItemClick(object sender, EventArgs e)
        {
            CloseTab(MainTab.SelectedIndex);
        }
        
        void CloseTab(int tabIndex)
        {
            SessionContext context = sessionViews[tabIndex].SessionContext;
            sessionViews[tabIndex].SessionContext.Close();
            
            sessionViews.RemoveAt(tabIndex);
            tabPages.RemoveAt(tabIndex);
            MainTab.TabPages.RemoveAt(tabIndex);
            OnClose(new CloseFileEventArgs(context));
            UpdateMenuItems();
        }

        void RunToolStripMenuItemItemClick(object sender, EventArgs e)
        {
            SessionViewForm sessionView = sessionViews[MainTab.SelectedIndex];
            sessionView.SessionContext.Execute();

        }

        void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SettingsDialog.ShowDialog();
        }

        void GuideToolStripMenuItemClick(object sender, EventArgs a)
        {
            System.Diagnostics.Process.Start("http://wiki.github.com/424f/lebowski/manual");
        }

        /// <summary>
        /// Raises the <see cref="ShareSession" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnShareSession(ShareSessionEventArgs e)
        {
            if (ShareSession != null)
            {
                ShareSession(this, e);
            }
        }

        
        /// <summary>
        /// Raises the <see cref="OpenFile" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnOpen(OpenFileEventArgs e)
        {
            if (OpenFile != null)
            {
                OpenFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CloseFile" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
      	protected virtual void OnClose(CloseFileEventArgs e)
        {
            if (CloseFile != null)
            {
                CloseFile(this, e);
            }
        }
     
        /// <summary>
        /// Raises the <see cref="SaveFile" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
      	
        protected virtual void OnSave(SaveFileEventArgs e)
        {
            if (SaveFile != null)
            {
                SaveFile(this, e);
            }
        }
        
        /// <summary>
        /// Raises the <see cref="ApplicationClosing" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnApplicationClosing(EventArgs e)
        {
            if (ApplicationClosing != null)
            {
                ApplicationClosing(this, e);
            }
        }
        
        /// <summary>
        /// Raises the <see cref="NewFile" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnNewFile(NewFileEventArgs e)
        {
            if (NewFile != null)
            {
                NewFile(this, e);
            }
        }
        
        /// <summary>
        /// Raises the <see cref="Participate" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>        
        protected virtual void OnParticipate(ParticipateEventArgs e)
        {
            if (Participate != null)
            {
                Participate(this, e);
            }
        }        
        
        /// <inheritdoc />
        public event EventHandler<EventArgs> ApplicationClosing;
        
        /// <inheritdoc />
        public event EventHandler<CloseFileEventArgs> CloseFile;
        
        /// <inheritdoc />
        public event EventHandler<NewFileEventArgs> NewFile;          
        
        /// <inheritdoc />
        public event EventHandler<OpenFileEventArgs> OpenFile;        
        
        /// <inheritdoc />
        public event EventHandler<SaveFileEventArgs> SaveFile;  
        
        /// <inheritdoc />
        public event EventHandler<ShareSessionEventArgs> ShareSession;                        
        
        /// <inheritdoc />
        public event EventHandler<ParticipateEventArgs> Participate;  
    }
}