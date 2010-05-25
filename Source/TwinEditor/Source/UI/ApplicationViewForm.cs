using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using Lebowski;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;
using TwinEditor.FileTypes;
using TwinEditor.UI;

using log4net;

namespace TwinEditor.UI
{
    public partial class ApplicationViewForm : Form, IApplicationView
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationViewForm));
        
        public event EventHandler<ShareSessionEventArgs> ShareSession;
        public event EventHandler<OpenEventArgs> Open;
        public event EventHandler<SaveEventArgs> Save;
        
        private Settings.SettingsDialog SettingsDialog;
        
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
                        CreateNewTab(currentFileType);
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
                        tabControls[MainTab.SelectedIndex].SessionContext.FileType = (IFileType)chooseMenuItem.Tag;
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
                    catch(Exception e)
                    {
                        Logger.WarnFormat("Could not load image for type {0}: {1}", fileType.Name, e);
                    }
                }
                MainTab.ImageList = imageList;                
            }   
        }
        private IFileType[] fileTypes;
        
        
        public ApplicationContext ApplicationContext { get; set; }
        
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
                            OnShareSession(new ShareSessionEventArgs(tabControls[MainTab.SelectedIndex].SessionContext, currentProtocol));
                        };
                        shareToolStripMenuItem.DropDown.Items.Add(menuItem);
                    }
                    
                    // Add menu item to join session
                    if (protocol.CanParticipate)
                    {
                        var menuItem = new ToolStripMenuItem(protocol.Name);
                        menuItem.Click += delegate 
                        {  
                            currentProtocol.Participate();
                        };
                        participateToolStripMenuItem.DropDown.Items.Add(menuItem);
                    }            
                }     
            }
        }
        private ICommunicationProtocol[] communicationProtocols;
        
        List<SessionViewForm> tabControls = new List<SessionViewForm>();
        List<TabPage> tabPages = new List<TabPage>();
        protected List<ToolStripMenuItem> chooseFileTypeMenuItems = new List<ToolStripMenuItem>();
        
        /// <summary>
        /// The number that is assigned to next new file without name
        /// </summary>
        private int nextFileNumber = 1;
        
        /// <summary>
        /// Updates GUI elements after the program state has changed
        /// </summary>
        public void UpdateGuiState()
        {
            UpdateMenuItems();
        }
        
        public void UpdateMenuItems()
        {
            bool fileOpen = MainTab.TabPages.Count > 0;
            
            copyToolStripMenuItem.Enabled = fileOpen;
            pasteToolStripMenuItem.Enabled = fileOpen;
            shareToolStripMenuItem.Enabled = fileOpen;
            cutToolStripMenuItem.Enabled = fileOpen;
            deleteToolStripMenuItem.Enabled = fileOpen;
            
            foreach (ToolStripMenuItem item in chooseFileTypeMenuItems)
            {
                item.Enabled = fileOpen;
                item.Checked = MainTab.SelectedIndex != -1 && item.Tag == tabControls[MainTab.SelectedIndex].SessionContext.FileType;
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
                var tab = tabControls[MainTab.SelectedIndex];
                compileToolStripMenuItem.Enabled = tab.SessionContext.FileType.CanCompile;
                runToolStripMenuItem.Enabled = tab.SessionContext.FileType.CanExecute;        
                shareToolStripMenuItem.Enabled = tab.SessionContext.State == SessionStates.Disconnected;
            }
        }
        
        public ApplicationViewForm()
        {            
            InitializeComponent();

            Logger.Info("MainForm component initialized.");
            //SourceCode.SetHighlighting("C#");
            
            // Clear tabs
            MainTab.TabPages.Clear();
            UpdateMenuItems();

            // Translate the menu
            TranslationUtil.TranslateMenuStrip(menuStrip1, ApplicationUtil.LanguageResources);
            
            // Configure settings dialog
            SettingsDialog = new Settings.SettingsDialog(ApplicationContext);
        }
        
        private void Translate(ToolStripMenuItem item, string id)
        {
            item.Text = ApplicationUtil.LanguageResources.GetString(id);
        }
        
        public ISessionView CreateNewSession(IFileType fileType)
        {
            return CreateNewTab(fileType);
        }
    
        public SessionViewForm CreateNewTab(IFileType fileType)
        {
            Logger.Info(string.Format("Creating new tab of file type {0}", fileType.Name));
            
            // Create a new tab and add a FileTabControl to it
            string filename = "New " + nextFileNumber++ + fileType.FileExtension;
            TabPage tabPage = new TabPage(filename);
            SessionViewForm tab = new SessionViewForm(this, tabPage);
            tabPage.Controls.Add(tab);
            tabControls.Add(tab);
            tabPages.Add(tabPage);
            MainTab.TabPages.Add(tabPage);
            
            tab.Dock = DockStyle.Fill;
            tab.SessionContext.FileType = fileType;
            tab.FileName = filename;
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
        
        void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            openFileDialog.RestoreDirectory = true;
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            
            // Create tab and initialize
            IFileType type = null;
            foreach (IFileType fileType in fileTypes)
            {
                fileType.FileNameMatches(openFileDialog.FileName);
                type = fileType;
            }
            
            // No appropriate file type has been found
            // Should never happen as we already filter file types in the openFileDialog
            if (type == null)
            {
                MessageBox.Show(
                    string.Format((TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxUnsupportedFileType") + " '{0}'"), openFileDialog.FileName),
                    TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxUnsupportedFileTypeCaption")
                );
                return;
            }
            
            OnOpen(new OpenEventArgs(openFileDialog.FileName, type));
            
            UpdateMenuItems();
        }
        
        void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
            tabControls[MainTab.SelectedIndex].SourceCode.PrintDocument.Print();
        }
        
        void MainTabSelected(object sender, TabControlEventArgs e)
        {
            /* After having selected a new tab, we have to update the 
            menus to reflect the actions that are possible */
            UpdateMenuItems();
        }
        
        void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {    
            SessionViewForm tabControl = tabControls[MainTab.SelectedIndex];
            SaveRequest(tabControl);
        }
        
        void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {    
            SessionViewForm tabControl = tabControls[MainTab.SelectedIndex];
            SaveWithDialog(tabControl);
        }
        
        // checks whether the tab is already on disk, and if yes calls 'Save' else calls 'SaveWithDialog'.
        void SaveRequest(SessionViewForm tabControl)
        {
            if (!tabControl.OnDisk)
            {
                SaveWithDialog(tabControl);
            }
            else
            {
                OnSave(new SaveEventArgs(tabControl, tabControl.FileName));
                tabControl.OnDisk = true;
                tabControl.FileModified = false;                    
                UpdateMenuItems();
                
            }
        }
        
        // opens saveFileDialog
        void SaveWithDialog(SessionViewForm tabControl)
        {
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = System.IO.Path.GetFileName(tabControl.FileName);
            saveFileDialog.Filter = string.Format("{0}|{1}", tabControl.SessionContext.FileType.Name, tabControl.SessionContext.FileType.FileNamePattern);
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tabControl.FileName = saveFileDialog.FileName;
                OnSave(new SaveEventArgs(tabControl, tabControl.FileName));
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
            foreach (SessionViewForm tabControl in tabControls)
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
        
        void CloseTab(int index) {
            SessionViewForm sessionView = tabControls[index];
            // check if the file has been modified since last save
            if (sessionView.FileModified)
            {    
                if (MessageBox.Show(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxOnCloseMessage"), TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxOnCloseCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveRequest(sessionView);
                }
            }
            sessionView.SessionContext.Close();
            Logger.Info(string.Format("{0} has been closed", sessionView.FileName));
            tabControls.RemoveAt(index);
            tabPages.RemoveAt(index);
            MainTab.TabPages.RemoveAt(index);
            UpdateMenuItems();
            
        }
        
        void RunToolStripMenuItemItemClick(object sender, EventArgs e)
        {
            SessionViewForm sessionView = tabControls[MainTab.SelectedIndex];        
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
        
        #region OnFoo event methods
        protected virtual void OnShareSession(ShareSessionEventArgs e)
        {
            if (ShareSession != null)
            {
                ShareSession(this, e);
            }
        }
        
        protected virtual void OnOpen(OpenEventArgs e)
        {
            if (Open != null)
            {
                Open(this, e);
            }
        }
        
        protected virtual void OnSave(SaveEventArgs e)
        {
            if (Save != null)
            {
                Save(this, e);
            }
        }
        #endregion
        
    }
}
