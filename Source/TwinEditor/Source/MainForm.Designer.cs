
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
			this.SuspendLayout();
			// 
			// SourceCode
			// 
			this.SourceCode.AutoScroll = true;
			this.SourceCode.IsReadOnly = false;
			this.SourceCode.Location = new System.Drawing.Point(13, 13);
			this.SourceCode.Margin = new System.Windows.Forms.Padding(0);
			this.SourceCode.Name = "SourceCode";
			this.SourceCode.Size = new System.Drawing.Size(613, 351);
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
			this.Name = "MainForm";
			this.Text = "TwinEditor";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button ChatSend;
		private System.Windows.Forms.TextBox ChatText;
		private System.Windows.Forms.TextBox ChatHistory;
		public System.Windows.Forms.Label label1;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
	}
}
