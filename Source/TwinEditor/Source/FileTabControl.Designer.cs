
namespace TwinEditor
{
	partial class FileTabControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.TabControl = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.SourceCode = new ICSharpCode.TextEditor.TextEditorControl();
			this.ChatSend = new System.Windows.Forms.Button();
			this.ChatHistory = new System.Windows.Forms.TextBox();
			this.ChatText = new System.Windows.Forms.TextBox();
			this.TabControl.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// TabControl
			// 
			this.TabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.TabControl.Controls.Add(this.tabPage3);
			this.TabControl.Location = new System.Drawing.Point(3, 3);
			this.TabControl.Name = "TabControl";
			this.TabControl.SelectedIndex = 0;
			this.TabControl.Size = new System.Drawing.Size(688, 372);
			this.TabControl.TabIndex = 9;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.SourceCode);
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(680, 346);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "Source";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// SourceCode
			// 
			this.SourceCode.AutoScroll = true;
			this.SourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SourceCode.IsReadOnly = false;
			this.SourceCode.Location = new System.Drawing.Point(3, 3);
			this.SourceCode.Margin = new System.Windows.Forms.Padding(0);
			this.SourceCode.Name = "SourceCode";
			this.SourceCode.Size = new System.Drawing.Size(674, 340);
			this.SourceCode.TabIndex = 1;
			// 
			// ChatSend
			// 
			this.ChatSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ChatSend.Location = new System.Drawing.Point(604, 470);
			this.ChatSend.Name = "ChatSend";
			this.ChatSend.Size = new System.Drawing.Size(87, 23);
			this.ChatSend.TabIndex = 8;
			this.ChatSend.Text = "Send";
			this.ChatSend.UseVisualStyleBackColor = true;
			// 
			// ChatHistory
			// 
			this.ChatHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ChatHistory.BackColor = System.Drawing.Color.White;
			this.ChatHistory.Location = new System.Drawing.Point(2, 381);
			this.ChatHistory.Multiline = true;
			this.ChatHistory.Name = "ChatHistory";
			this.ChatHistory.ReadOnly = true;
			this.ChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ChatHistory.Size = new System.Drawing.Size(686, 84);
			this.ChatHistory.TabIndex = 6;
			// 
			// ChatText
			// 
			this.ChatText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ChatText.Location = new System.Drawing.Point(2, 473);
			this.ChatText.Name = "ChatText";
			this.ChatText.Size = new System.Drawing.Size(596, 20);
			this.ChatText.TabIndex = 7;
			// 
			// FileTabControl
			// 
			this.Controls.Add(this.TabControl);
			this.Controls.Add(this.ChatSend);
			this.Controls.Add(this.ChatHistory);
			this.Controls.Add(this.ChatText);
			this.Name = "FileTabControl";
			this.Size = new System.Drawing.Size(691, 496);
			this.TabControl.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public System.Windows.Forms.TabControl TabControl;
		private System.Windows.Forms.TextBox ChatText;
		private System.Windows.Forms.TextBox ChatHistory;
		private System.Windows.Forms.Button ChatSend;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
		private System.Windows.Forms.TabPage tabPage3;
	}
}
