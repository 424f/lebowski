
namespace TwinEditor
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.SourceCode = new ICSharpCode.TextEditor.TextEditorControl();
			this.label1 = new System.Windows.Forms.Label();
			this.ChatHistory = new System.Windows.Forms.TextBox();
			this.ChatText = new System.Windows.Forms.TextBox();
			this.ChatSend = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// SourceCode
			// 
			this.SourceCode.AutoScroll = true;
			this.SourceCode.IsReadOnly = false;
			this.SourceCode.Location = new System.Drawing.Point(13, 24);
			this.SourceCode.Margin = new System.Windows.Forms.Padding(0);
			this.SourceCode.Name = "SourceCode";
			this.SourceCode.Size = new System.Drawing.Size(613, 340);
			this.SourceCode.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 489);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(445, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Information";
			// 
			// ChatHistory
			// 
			this.ChatHistory.BackColor = System.Drawing.Color.White;
			this.ChatHistory.Location = new System.Drawing.Point(13, 367);
			this.ChatHistory.Multiline = true;
			this.ChatHistory.Name = "ChatHistory";
			this.ChatHistory.ReadOnly = true;
			this.ChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ChatHistory.Size = new System.Drawing.Size(613, 92);
			this.ChatHistory.TabIndex = 2;
			// 
			// ChatText
			// 
			this.ChatText.Location = new System.Drawing.Point(13, 466);
			this.ChatText.Name = "ChatText";
			this.ChatText.Size = new System.Drawing.Size(533, 20);
			this.ChatText.TabIndex = 3;
			this.ChatText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyDown);
			this.ChatText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyUp);
			this.ChatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChatTextKeyPress);
			// 
			// ChatSend
			// 
			this.ChatSend.Location = new System.Drawing.Point(552, 465);
			this.ChatSend.Name = "ChatSend";
			this.ChatSend.Size = new System.Drawing.Size(75, 20);
			this.ChatSend.TabIndex = 4;
			this.ChatSend.Text = "Send";
			this.ChatSend.UseVisualStyleBackColor = true;
			this.ChatSend.Click += new System.EventHandler(this.ChatSendClick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.fileToolStripMenuItem,
									this.editToolStripMenuItem,
									this.scriptToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(639, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.newToolStripMenuItem,
									this.openToolStripMenuItem,
									this.closeToolStripMenuItem,
									this.toolStripMenuItem2,
									this.saveToolStripMenuItem,
									this.saveAsToolStripMenuItem,
									this.saveAllToolStripMenuItem,
									this.toolStripMenuItem3,
									this.printToolStripMenuItem,
									this.toolStripMenuItem4,
									this.recentFilesToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "Open";
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.closeToolStripMenuItem.Text = "Close";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem2.Text = "--";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			// 
			// saveAllToolStripMenuItem
			// 
			this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
			this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAllToolStripMenuItem.Text = "Save All";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem3.Text = "--";
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.printToolStripMenuItem.Text = "Print";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem4.Text = "--";
			// 
			// recentFilesToolStripMenuItem
			// 
			this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
			this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.recentFilesToolStripMenuItem.Text = "Recent Files";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.cutToolStripMenuItem,
									this.copyToolStripMenuItem,
									this.pasteToolStripMenuItem,
									this.deleteToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			// 
			// scriptToolStripMenuItem
			// 
			this.scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.compileToolStripMenuItem,
									this.runToolStripMenuItem});
			this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
			this.scriptToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.scriptToolStripMenuItem.Text = "Script";
			// 
			// compileToolStripMenuItem
			// 
			this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			this.compileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.compileToolStripMenuItem.Text = "Compile";
			// 
			// runToolStripMenuItem
			// 
			this.runToolStripMenuItem.Name = "runToolStripMenuItem";
			this.runToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.runToolStripMenuItem.Text = "Run";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(639, 521);
			this.Controls.Add(this.ChatSend);
			this.Controls.Add(this.ChatText);
			this.Controls.Add(this.ChatHistory);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SourceCode);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "TwinEditor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Button ChatSend;
		private System.Windows.Forms.TextBox ChatText;
		private System.Windows.Forms.TextBox ChatHistory;
		public System.Windows.Forms.Label label1;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
	}
}
