
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileTabControl));
			this.TabControl = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.SourceCode = new ICSharpCode.TextEditor.TextEditorControl();
			this.ChatHistory = new System.Windows.Forms.TextBox();
			this.ChatText = new System.Windows.Forms.TextBox();
			this.connectionStatusLabel = new System.Windows.Forms.Label();
			this.connectionStatusPicture = new System.Windows.Forms.PictureBox();
			this.TabControl.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.connectionStatusPicture)).BeginInit();
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
			this.TabControl.Size = new System.Drawing.Size(516, 592);
			this.TabControl.TabIndex = 9;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.SourceCode);
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(508, 566);
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
			this.SourceCode.Size = new System.Drawing.Size(502, 560);
			this.SourceCode.TabIndex = 1;
			// 
			// ChatHistory
			// 
			this.ChatHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ChatHistory.BackColor = System.Drawing.Color.White;
			this.ChatHistory.Location = new System.Drawing.Point(522, 6);
			this.ChatHistory.Multiline = true;
			this.ChatHistory.Name = "ChatHistory";
			this.ChatHistory.ReadOnly = true;
			this.ChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ChatHistory.Size = new System.Drawing.Size(157, 540);
			this.ChatHistory.TabIndex = 6;
			// 
			// ChatText
			// 
			this.ChatText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ChatText.Location = new System.Drawing.Point(522, 552);
			this.ChatText.Name = "ChatText";
			this.ChatText.Size = new System.Drawing.Size(157, 22);
			this.ChatText.TabIndex = 7;
			this.ChatText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyDown);
			this.ChatText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyUp);
			this.ChatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChatTextKeyPress);
			// 
			// connectionStatusLabel
			// 
			this.connectionStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStatusLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.connectionStatusLabel.Location = new System.Drawing.Point(549, 574);
			this.connectionStatusLabel.Name = "connectionStatusLabel";
			this.connectionStatusLabel.Size = new System.Drawing.Size(100, 24);
			this.connectionStatusLabel.TabIndex = 10;
			this.connectionStatusLabel.Text = "Disconnected";
			this.connectionStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// connectionStatusPicture
			// 
			this.connectionStatusPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStatusPicture.Image = ((System.Drawing.Image)(resources.GetObject("connectionStatusPicture.Image")));
			this.connectionStatusPicture.Location = new System.Drawing.Point(655, 574);
			this.connectionStatusPicture.Name = "connectionStatusPicture";
			this.connectionStatusPicture.Size = new System.Drawing.Size(24, 24);
			this.connectionStatusPicture.TabIndex = 11;
			this.connectionStatusPicture.TabStop = false;
			// 
			// FileTabControl
			// 
			this.Controls.Add(this.connectionStatusPicture);
			this.Controls.Add(this.connectionStatusLabel);
			this.Controls.Add(this.TabControl);
			this.Controls.Add(this.ChatHistory);
			this.Controls.Add(this.ChatText);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "FileTabControl";
			this.Size = new System.Drawing.Size(682, 598);
			this.TabControl.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.connectionStatusPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.PictureBox connectionStatusPicture;
		private System.Windows.Forms.Label connectionStatusLabel;
		public System.Windows.Forms.TabControl TabControl;
		private System.Windows.Forms.TextBox ChatText;
		private System.Windows.Forms.TextBox ChatHistory;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
		private System.Windows.Forms.TabPage tabPage3;
	}
}
