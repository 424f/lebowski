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
using TwinEditor.UI.FileTypes;
using log4net;

namespace TwinEditor
{
	public partial class MainForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));
		
		public const string MESSAGE_CLOSE = "Save changes?";
		public const string CAPTION_CLOSE = "Save";
		
		protected IFileType[] fileTypes;
		protected ICommunicationProtocol[] protocols;
		List<SessionTabControl> tabControls = new List<SessionTabControl>();
		List<TabPage> tabPages = new List<TabPage>();
		Controller controller;
		
		/// <summary>
		/// Updates GUI elements after the program state has changed
		/// </summary>
		public void UpdateGuiState()
		{
		    UpdateMenuItems();
		}
		
		public void UpdateMenuItems()
		{
			if(MainTab.TabPages.Count == 0)
			{
				editToolStripMenuItem.Enabled = false;
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
				editToolStripMenuItem.Enabled = true;
				scriptToolStripMenuItem.Enabled = true;
				printToolStripMenuItem.Enabled = true;
				var tab = tabControls[MainTab.SelectedIndex];
				compileToolStripMenuItem.Enabled = tab.FileType.CanCompile;
				runToolStripMenuItem.Enabled = tab.FileType.CanExecute;			
			}
		}
		
		public MainForm(Controller controller)
		{			
			this.controller = controller;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			Logger.Info("MainForm component initialized.");
			//SourceCode.SetHighlighting("C#");
			
			// Clear tabs
			MainTab.TabPages.Clear();
			UpdateMenuItems();

			// Translate the menu
			TranslationUtil.TranslateMenuStrip(menuStrip1, ApplicationUtil.LanguageResources);
			
			// Supported file types
			openFileDialog.Filter = "";
			fileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType))
				.Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
				.Cast<IFileType>()
				.ToArray();
			foreach(IFileType fileType in fileTypes)
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
				if(openFileDialog.Filter.Length > 0)
				{
					newFilter = "|" + newFilter;
				}
				openFileDialog.Filter += newFilter;
			}
			
			// Supported communication protocols
			protocols = ExtensionUtil.FindTypesImplementing(typeof(ICommunicationProtocol))
				.Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
				.Cast<ICommunicationProtocol>()
				.Where((p) => p.Enabled)
				.ToArray();
			foreach(ICommunicationProtocol protocol in protocols)
			{
				ICommunicationProtocol currentProtocol = protocol;
				
				// Add menu item to share session
				if(protocol.CanShare)
				{
					var menuItem = new ToolStripMenuItem(protocol.Name);
					menuItem.Click += delegate 
					{  
						currentProtocol.Share(tabControls[MainTab.SelectedIndex]);
					};
					shareToolStripMenuItem.DropDown.Items.Add(menuItem);
				}
				
				// Add menu item to join session
				if(protocol.CanParticipate)
				{
					var menuItem = new ToolStripMenuItem(protocol.Name);
					menuItem.Click += delegate 
					{  
						currentProtocol.Participate();
					};
					participateToolStripMenuItem.DropDown.Items.Add(menuItem);
				}
				
				// Register to communication protocol events
				currentProtocol.HostSession += delegate(object sender, HostSessionEventArgs e)
				{ 
					MultichannelConnection mcc = new MultichannelConnection(e.Connection);
					var syncConnection = mcc.CreateChannel();
					var sync = new DifferentialSynchronizationStrategy(0, e.Session.Context, syncConnection);

					e.Session.StartSession(sync, mcc.CreateChannel());
					
				};
				
				currentProtocol.JoinSession += delegate(object sender, JoinSessionEventArgs e)
				{ 
					// TODO: choose correct file type
					SessionTabControl tab = CreateNewTab(fileTypes[0]);
					
					MultichannelConnection mcc = new MultichannelConnection(e.Connection);
					var syncConnection = mcc.CreateChannel();
					var sync = new DifferentialSynchronizationStrategy(1, tab.Context, syncConnection);
					tab.StartSession(sync, mcc.CreateChannel());
					
				};				
			}
		}
		
		private void Translate(ToolStripMenuItem item, string id)
		{
		    item.Text = ApplicationUtil.LanguageResources.GetString(id);
		}
	
		public SessionTabControl CreateNewTab(IFileType fileType)
		{
			Logger.Info(string.Format("Creating new tab of file type {0}", fileType.Name));
			
			// Create a new tab and add a FileTabControl to it
			string filename = "New " + controller.GetNextFileNumber() + fileType.FileExtension;
			TabPage tabPage = new TabPage(filename);
			SessionTabControl tab = new SessionTabControl();
			tab.Dock = DockStyle.Fill;
			tab.FileType = fileType;
			tab.FileName = filename;
			tabPage.Controls.Add(tab);
			tabControls.Add(tab);
			tabPages.Add(tabPage);
			MainTab.TabPages.Add(tabPage);
			MainTab.SelectedTab = tabPage;
			UpdateMenuItems();
			return tab;
		}
		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFileDialog.RestoreDirectory = true;
			DialogResult result = openFileDialog.ShowDialog();
			if(result != DialogResult.OK)
				return;
			
			// Create tab and initialize
			IFileType type = null;
			foreach(IFileType fileType in fileTypes)
			{
				fileType.FileNameMatches(openFileDialog.FileName);
				type = fileType;
			}
			
			// No appropriate file type has been found
			if(type == null)
			{
				MessageBox.Show(
					string.Format("We're sorry, but Lebowski does not currently support the file '{0}'", openFileDialog.FileName),
					"Could not open file"
				);
				return;
			}
			
			// Create a new tab for the file
			SessionTabControl tab = CreateNewTab(type);
			tab.FileName = openFileDialog.FileName;
			tab.FileType = type;
			
			// Put content into editor
			// case when user cancelled dialog not handled yet
			string content = File.ReadAllText(openFileDialog.FileName);
			tab.SourceCode.Text = content;
			tabPages.Last().Text = openFileDialog.FileName;
			
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
			SessionTabControl tabControl = tabControls[MainTab.SelectedIndex];
			SaveRequest(tabControl);
		}
		
		void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
		{	
			SessionTabControl tabControl = tabControls[MainTab.SelectedIndex];
			SaveWithDialog(tabControl);
		}
		
		// checks whether the tab is already on disk, and if yes calls 'Save' else calls 'SaveWithDialog'.
		void SaveRequest(SessionTabControl tabControl) {
			if(!tabControl.OnDisk)
			{
				SaveWithDialog(tabControl);
			}
			else
			{
				Save(tabControl);
			}
		}
		
		// opens saveFileDialog
		void SaveWithDialog(SessionTabControl tabControl) {
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = System.IO.Path.GetFileName(tabControl.FileName);
			saveFileDialog.Filter = string.Format("{0}|{1}", tabControl.FileType.Name, tabControl.FileType.FileNamePattern);
			DialogResult result = saveFileDialog.ShowDialog();
			if(result == DialogResult.OK)
			{
				tabControl.FileName = saveFileDialog.FileName;
				Save(tabControl);
			}
			else
			{
				Logger.Info(string.Format("User cancelled saving action {0}", saveFileDialog.FileName));
			}
		}
		
		// saves file
		void Save(SessionTabControl tabControl)
		{			
			try
			{
				File.WriteAllText(tabControl.FileName, tabControl.SourceCode.Text);
				Logger.Info(string.Format("{0} has been saved successfully", tabControl.FileName));
				((TabPage)tabControl.Parent).Text = System.IO.Path.GetFileName(tabControl.FileName);
				tabControl.OnDisk = true;
				tabControl.FileModified = false;
				UpdateMenuItems();
			}
			catch (Exception e)
			{
				Logger.Error(string.Format("{0} could not be saved due to {1}", saveFileDialog.FileName, e.Message));
			}
		}
		
		void SaveAllToolStripMenuItemClick(object sender, EventArgs e)
		{
			foreach(SessionTabControl tabControl in tabControls)
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
			SessionTabControl tabControl = tabControls[MainTab.SelectedIndex];
			if (tabControl.FileModified)
			{
				if (MessageBox.Show(MESSAGE_CLOSE, CAPTION_CLOSE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					SaveRequest(tabControl);
				}
			}
			tabControl.Close();
			Logger.Info(string.Format("{0} has been closed", tabControl.FileName));
			tabControls.RemoveAt(MainTab.SelectedIndex);
			tabPages.RemoveAt(MainTab.SelectedIndex);
			MainTab.TabPages.Remove(MainTab.SelectedTab);
			UpdateMenuItems();
		}
		
		void RunToolStripMenuItemItemClick(object sender, EventArgs e) {
			SessionTabControl tabControl = tabControls[MainTab.SelectedIndex];		
			StringWriter writer = new StringWriter();
			tabControl.FileType.Execute(tabControl.SourceCode.Text, writer);
			
			// Create new executions tab
			tabControl.IncrementNumExections();
			TabPage newPage = new TabPage(string.Format("Execution #{0}", tabControl.NumExecutions));
			ExecutionTabControl execution = new ExecutionTabControl();
			execution.SetStandardOutput(writer.GetStringBuilder().ToString());
			newPage.Controls.Add(execution);
			execution.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl.TabControl.TabPages.Add(newPage);
			tabControl.TabControl.SelectedTab = newPage;
		}
	}
}
