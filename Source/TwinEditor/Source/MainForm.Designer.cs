
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.SourceCode = new ICSharpCode.TextEditor.TextEditorControl();
			this.ChatHistory = new System.Windows.Forms.TextBox();
			this.ChatText = new System.Windows.Forms.TextBox();
			this.ChatSend = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textFiletxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pythonpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.shareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tCPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.skypeAP2APToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.johndoeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.foobarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.connectToSharedDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tCPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.uDPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.skypeAP2APToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.guideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainTabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.menuStrip1.SuspendLayout();
			this.MainTabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// SourceCode
			// 
			this.SourceCode.AutoScroll = true;
			this.SourceCode.IsReadOnly = false;
			this.SourceCode.Location = new System.Drawing.Point(9, 12);
			this.SourceCode.Margin = new System.Windows.Forms.Padding(0);
			this.SourceCode.Name = "SourceCode";
			this.SourceCode.Size = new System.Drawing.Size(613, 110);
			this.SourceCode.TabIndex = 0;
			// 
			// ChatHistory
			// 
			this.ChatHistory.BackColor = System.Drawing.Color.White;
			this.ChatHistory.Location = new System.Drawing.Point(6, 125);
			this.ChatHistory.Multiline = true;
			this.ChatHistory.Name = "ChatHistory";
			this.ChatHistory.ReadOnly = true;
			this.ChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ChatHistory.Size = new System.Drawing.Size(613, 92);
			this.ChatHistory.TabIndex = 2;
			// 
			// ChatText
			// 
			this.ChatText.Location = new System.Drawing.Point(6, 224);
			this.ChatText.Name = "ChatText";
			this.ChatText.Size = new System.Drawing.Size(533, 20);
			this.ChatText.TabIndex = 3;
			this.ChatText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyDown);
			this.ChatText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyUp);
			this.ChatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChatTextKeyPress);
			// 
			// ChatSend
			// 
			this.ChatSend.Location = new System.Drawing.Point(545, 223);
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
									this.scriptToolStripMenuItem,
									this.helpToolStripMenuItem});
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
									this.toolStripSeparator1,
									this.saveToolStripMenuItem,
									this.saveAsToolStripMenuItem,
									this.saveAllToolStripMenuItem,
									this.toolStripSeparator2,
									this.printToolStripMenuItem,
									this.toolStripSeparator3,
									this.recentFilesToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.textFiletxtToolStripMenuItem,
									this.pythonpyToolStripMenuItem});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItemClick);
			// 
			// textFiletxtToolStripMenuItem
			// 
			this.textFiletxtToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("textFiletxtToolStripMenuItem.Image")));
			this.textFiletxtToolStripMenuItem.Name = "textFiletxtToolStripMenuItem";
			this.textFiletxtToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.textFiletxtToolStripMenuItem.Text = "Text file (*.txt)";
			// 
			// pythonpyToolStripMenuItem
			// 
			this.pythonpyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pythonpyToolStripMenuItem.Image")));
			this.pythonpyToolStripMenuItem.Name = "pythonpyToolStripMenuItem";
			this.pythonpyToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.pythonpyToolStripMenuItem.Text = "Python (*.py)";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.openToolStripMenuItem.Text = "Open";
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.closeToolStripMenuItem.Text = "Close";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			// 
			// saveAllToolStripMenuItem
			// 
			this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
			this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
									| System.Windows.Forms.Keys.S)));
			this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.saveAllToolStripMenuItem.Text = "Save All";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.printToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.printToolStripMenuItem.Text = "Print";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(184, 6);
			// 
			// recentFilesToolStripMenuItem
			// 
			this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
			this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.recentFilesToolStripMenuItem.Text = "Recent Files";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.cutToolStripMenuItem,
									this.copyToolStripMenuItem,
									this.pasteToolStripMenuItem,
									this.deleteToolStripMenuItem,
									this.toolStripSeparator4,
									this.shareToolStripMenuItem,
									this.connectToSharedDocumentToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(167, 6);
			// 
			// shareToolStripMenuItem
			// 
			this.shareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tCPToolStripMenuItem,
									this.uDPToolStripMenuItem,
									this.skypeAP2APToolStripMenuItem});
			this.shareToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("shareToolStripMenuItem.Image")));
			this.shareToolStripMenuItem.Name = "shareToolStripMenuItem";
			this.shareToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.shareToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.shareToolStripMenuItem.Text = "Share";
			// 
			// tCPToolStripMenuItem
			// 
			this.tCPToolStripMenuItem.Name = "tCPToolStripMenuItem";
			this.tCPToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.tCPToolStripMenuItem.Text = "TCP";
			// 
			// uDPToolStripMenuItem
			// 
			this.uDPToolStripMenuItem.Name = "uDPToolStripMenuItem";
			this.uDPToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.uDPToolStripMenuItem.Text = "UDP";
			// 
			// skypeAP2APToolStripMenuItem
			// 
			this.skypeAP2APToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.johndoeToolStripMenuItem,
									this.foobarToolStripMenuItem});
			this.skypeAP2APToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("skypeAP2APToolStripMenuItem.Image")));
			this.skypeAP2APToolStripMenuItem.Name = "skypeAP2APToolStripMenuItem";
			this.skypeAP2APToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.skypeAP2APToolStripMenuItem.Text = "Skype AP2AP";
			// 
			// johndoeToolStripMenuItem
			// 
			this.johndoeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("johndoeToolStripMenuItem.Image")));
			this.johndoeToolStripMenuItem.Name = "johndoeToolStripMenuItem";
			this.johndoeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.johndoeToolStripMenuItem.Text = "john.doe";
			// 
			// foobarToolStripMenuItem
			// 
			this.foobarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("foobarToolStripMenuItem.Image")));
			this.foobarToolStripMenuItem.Name = "foobarToolStripMenuItem";
			this.foobarToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.foobarToolStripMenuItem.Text = "foo.bar";
			// 
			// connectToSharedDocumentToolStripMenuItem
			// 
			this.connectToSharedDocumentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tCPToolStripMenuItem1,
									this.uDPToolStripMenuItem1,
									this.skypeAP2APToolStripMenuItem1});
			this.connectToSharedDocumentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("connectToSharedDocumentToolStripMenuItem.Image")));
			this.connectToSharedDocumentToolStripMenuItem.Name = "connectToSharedDocumentToolStripMenuItem";
			this.connectToSharedDocumentToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
			this.connectToSharedDocumentToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.connectToSharedDocumentToolStripMenuItem.Text = "Participate";
			// 
			// tCPToolStripMenuItem1
			// 
			this.tCPToolStripMenuItem1.Name = "tCPToolStripMenuItem1";
			this.tCPToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.tCPToolStripMenuItem1.Text = "TCP";
			// 
			// uDPToolStripMenuItem1
			// 
			this.uDPToolStripMenuItem1.Name = "uDPToolStripMenuItem1";
			this.uDPToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.uDPToolStripMenuItem1.Text = "UDP";
			// 
			// skypeAP2APToolStripMenuItem1
			// 
			this.skypeAP2APToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("skypeAP2APToolStripMenuItem1.Image")));
			this.skypeAP2APToolStripMenuItem1.Name = "skypeAP2APToolStripMenuItem1";
			this.skypeAP2APToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.skypeAP2APToolStripMenuItem1.Text = "Skype AP2AP";
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
			this.compileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("compileToolStripMenuItem.Image")));
			this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.compileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.compileToolStripMenuItem.Text = "Compile";
			// 
			// runToolStripMenuItem
			// 
			this.runToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("runToolStripMenuItem.Image")));
			this.runToolStripMenuItem.Name = "runToolStripMenuItem";
			this.runToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.runToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.runToolStripMenuItem.Text = "Run";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.guideToolStripMenuItem,
									this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// guideToolStripMenuItem
			// 
			this.guideToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("guideToolStripMenuItem.Image")));
			this.guideToolStripMenuItem.Name = "guideToolStripMenuItem";
			this.guideToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.guideToolStripMenuItem.Text = "Guide";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "About...";
			// 
			// MainTabControl
			// 
			this.MainTabControl.Controls.Add(this.tabPage1);
			this.MainTabControl.Controls.Add(this.tabPage2);
			this.MainTabControl.Location = new System.Drawing.Point(0, 27);
			this.MainTabControl.Name = "MainTabControl";
			this.MainTabControl.SelectedIndex = 0;
			this.MainTabControl.Size = new System.Drawing.Size(639, 503);
			this.MainTabControl.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.SourceCode);
			this.tabPage1.Controls.Add(this.ChatSend);
			this.tabPage1.Controls.Add(this.ChatHistory);
			this.tabPage1.Controls.Add(this.ChatText);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(631, 477);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "file.py";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(192, 74);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(639, 532);
			this.Controls.Add(this.MainTabControl);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "TwinEditor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.MainTabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl MainTabControl;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem guideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pythonpyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem textFiletxtToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem skypeAP2APToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem uDPToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem tCPToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem connectToSharedDocumentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem foobarToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem johndoeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem skypeAP2APToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uDPToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tCPToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shareToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Button ChatSend;
		private System.Windows.Forms.TextBox ChatText;
		private System.Windows.Forms.TextBox ChatHistory;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
	}
}
