using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.Net;
using Lebowski.UI.FileTypes;
using log4net;

namespace TwinEditor
{
	public partial class MainForm : Form
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));
		
		IConnection chatConnection;
		protected IFileType[] fileTypes;
		protected ICommunicationProtocol[] protocols;
		List<FileTabControl> tabControls = new List<FileTabControl>();
		List<TabPage> tabPages = new List<TabPage>();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			Logger.Info("MainForm component initialized.");
			//SourceCode.SetHighlighting("C#");
			
			// Clear tabs
			MainTab.TabPages.Clear();
			
			// Supported file types
			openFileDialog.Filter = "";
			fileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType)).Select(
				(t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{})
			).Cast<IFileType>().ToArray();
			foreach(IFileType fileType in fileTypes)
			{
				var menuItem = new ToolStripMenuItem(fileType.Name + " (" + fileType.FileNamePattern + ")");
				menuItem.Click += delegate
				{
					CreateNewTab(fileType);
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
			protocols = ExtensionUtil.FindTypesImplementing(typeof(ICommunicationProtocol)).Select(
				(t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{})
			).Cast<ICommunicationProtocol>().ToArray();			
			foreach(ICommunicationProtocol protocol in protocols)
			{
				var menuItem = new ToolStripMenuItem(protocol.Name);
				shareToolStripMenuItem.DropDown.Items.Add(menuItem);
			}
			
		}
		
		public void SetChatConnection(IConnection connection)
		{
			if(chatConnection != null)
				throw new Exception("There already is a chat connection");
			this.chatConnection = connection;
			
			this.chatConnection.Received += delegate(object sender, ReceivedEventArgs e) {
				string s = (string)e.Message;
				ChatText.Invoke((Action)delegate { AddChatMessage(s); });
			};
		}
		
		void ChatSendClick(object sender, EventArgs e)
		{
			if(ChatText.Text.Length == 0)
				return;
			chatConnection.Send(ChatText.Text);
			AddChatMessage(ChatText.Text);
			ChatText.Text = "";
		}
		
		void AddChatMessage(string text)
		{
			ChatHistory.AppendText(text + Environment.NewLine);
		}
		
		void ChatTextKeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				ChatSendClick(this, null);
			}
		}
		
		void ChatTextKeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				e.Handled = true;
			}
		}
		
		void ChatTextKeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) 13)
			{
				e.Handled = true;
			}
			else
			{
				base.OnKeyPress(e);
			}
		}
		
		void NewToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		
		
		
		public FileTabControl CreateNewTab(IFileType fileType)
		{
			Logger.Info(string.Format("Creating new tab of file type {0}", fileType.Name));
			
			// Create a new tab and add a FileTabControl to it
			TabPage tabPage = new TabPage("???");
			FileTabControl tab = new FileTabControl();
			tab.Dock = DockStyle.Fill;
			tab.FileType = fileType;
			tabPage.Controls.Add(tab);
			tabControls.Add(tab);
			tabPages.Add(tabPage);
			MainTab.TabPages.Add(tabPage);
			return tab;
		}
		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFileDialog.RestoreDirectory = true;
			openFileDialog.ShowDialog();
			MessageBox.Show("You selected: " + openFileDialog.FileName);
			
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
			FileTabControl tab = CreateNewTab(type);
			tab.FileName = openFileDialog.FileName;
			tab.FileType = type;
			
			// Put content into editor
			string content = File.ReadAllText(openFileDialog.FileName);
			tab.SourceCode.Text = content;
			tabPages.Last().Text = openFileDialog.FileName;
		}
		
		void PrintToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControls[MainTab.SelectedIndex].SourceCode.PrintDocument.Print();
		}
		
		void MainTabSelected(object sender, TabControlEventArgs e)
		{
			/* After having selected a new tab, we have to update the 
			menus to reflect the actions that are possible */
			var tab = tabControls[e.TabPageIndex];
			compileToolStripMenuItem.Enabled = tab.FileType.CanCompile;
			runToolStripMenuItem.Enabled = tab.FileType.CanExecute;
			
		}
		
		void SaveToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(tabControls[MainTab.SelectedIndex].FileName == null)
			{
				SaveAsToolStripMenuItemClick(sender, e);
				return;
			}
			
			SaveFile(tabControls[MainTab.SelectedIndex]);
			
		}
		
		void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
		{
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.ShowDialog();
			
			SaveFile(tabControls[MainTab.SelectedIndex]);
		}
		
		void SaveFile(FileTabControl tabControl)
		{
			File.WriteAllText(tabControl.FileName, tabControls[MainTab.SelectedIndex].SourceCode.Text);			
			tabControls[MainTab.SelectedIndex].FileName = saveFileDialog.FileName;
			tabPages[MainTab.SelectedIndex].Text = System.IO.Path.GetFileName(saveFileDialog.FileName);			
			
		}
	}
}
