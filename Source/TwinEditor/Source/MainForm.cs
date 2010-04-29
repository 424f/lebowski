using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Lebowski;
using Lebowski.Net;
using Lebowski.UI.FileTypes;

namespace TwinEditor
{
	public partial class MainForm : Form
	{
		IConnection chatConnection;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//SourceCode.SetHighlighting("C#");
			
			// Supported file types
			IFileType[] fileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType)).Select(
				(t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{})
			).Cast<IFileType>().ToArray();
			foreach(IFileType fileType in fileTypes)
			{
				newToolStripMenuItem.DropDown.Items.Add(new ToolStripMenuItem(fileType.Name + " (" + fileType.FileNamePattern + ")"));
			}
			
			// Supported communication protocols
			ICommunicationProtocol[] protocols = ExtensionUtil.FindTypesImplementing(typeof(ICommunicationProtocol)).Select(
				(t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{})
			).Cast<ICommunicationProtocol>().ToArray();			
			foreach(ICommunicationProtocol protocol in protocols)
			{
				shareToolStripMenuItem.DropDown.Items.Add(new ToolStripMenuItem(protocol.Name));
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
	}
}
