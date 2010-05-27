
namespace TwinEditor.UI
{
    partial class SessionViewForm
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionViewForm));
        	this.connectionStopWaitingButton = new System.Windows.Forms.Button();
        	this.ChatHistory = new System.Windows.Forms.TextBox();
        	this.ChatText = new System.Windows.Forms.TextBox();
        	this.connectionStatusLabel = new System.Windows.Forms.Label();
        	this.connectionStatusPicture = new System.Windows.Forms.PictureBox();
        	this.splitContainer = new System.Windows.Forms.SplitContainer();
        	this.TabControl = new TwinEditor.UI.ClosableTabControl();
        	this.tabPage3 = new System.Windows.Forms.TabPage();
        	this.SourceCode = new ICSharpCode.TextEditor.TextEditorControl();
        	((System.ComponentModel.ISupportInitialize)(this.connectionStatusPicture)).BeginInit();
        	this.splitContainer.Panel1.SuspendLayout();
        	this.splitContainer.Panel2.SuspendLayout();
        	this.splitContainer.SuspendLayout();
        	this.TabControl.SuspendLayout();
        	this.tabPage3.SuspendLayout();
        	this.SuspendLayout();
        	//
        	// connectionStopWaitingButton
        	//
        	this.connectionStopWaitingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.connectionStopWaitingButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.connectionStopWaitingButton.Image = ((System.Drawing.Image)(resources.GetObject("connectionStopWaitingButton.Image")));
        	this.connectionStopWaitingButton.Location = new System.Drawing.Point(153, 483);
        	this.connectionStopWaitingButton.Name = "connectionStopWaitingButton";
        	this.connectionStopWaitingButton.Size = new System.Drawing.Size(20, 20);
        	this.connectionStopWaitingButton.TabIndex = 12;
        	this.connectionStopWaitingButton.UseVisualStyleBackColor = true;
        	this.connectionStopWaitingButton.Visible = false;
        	this.connectionStopWaitingButton.Click += new System.EventHandler(this.ConnectionStopWaitingButtonClick);
        	//
        	// ChatHistory
        	//
        	this.ChatHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        	        	        	| System.Windows.Forms.AnchorStyles.Left)
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.ChatHistory.BackColor = System.Drawing.Color.White;
        	this.ChatHistory.Location = new System.Drawing.Point(0, 0);
        	this.ChatHistory.Multiline = true;
        	this.ChatHistory.Name = "ChatHistory";
        	this.ChatHistory.ReadOnly = true;
        	this.ChatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        	this.ChatHistory.Size = new System.Drawing.Size(175, 458);
        	this.ChatHistory.TabIndex = 6;
        	//
        	// ChatText
        	//
        	this.ChatText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.ChatText.Location = new System.Drawing.Point(0, 459);
        	this.ChatText.Name = "ChatText";
        	this.ChatText.Size = new System.Drawing.Size(175, 22);
        	this.ChatText.TabIndex = 7;
        	this.ChatText.TextChanged += new System.EventHandler(this.ChatTextTextChanged);
        	this.ChatText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyDown);
        	this.ChatText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChatTextKeyUp);
        	this.ChatText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChatTextKeyPress);
        	//
        	// connectionStatusLabel
        	//
        	this.connectionStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.connectionStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.connectionStatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.connectionStatusLabel.Location = new System.Drawing.Point(19, 480);
        	this.connectionStatusLabel.Name = "connectionStatusLabel";
        	this.connectionStatusLabel.Size = new System.Drawing.Size(108, 24);
        	this.connectionStatusLabel.TabIndex = 10;
        	this.connectionStatusLabel.Text = "Disconnected";
        	this.connectionStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	//
        	// connectionStatusPicture
        	//
        	this.connectionStatusPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.connectionStatusPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.connectionStatusPicture.Image = ((System.Drawing.Image)(resources.GetObject("connectionStatusPicture.Image")));
        	this.connectionStatusPicture.Location = new System.Drawing.Point(133, 485);
        	this.connectionStatusPicture.Name = "connectionStatusPicture";
        	this.connectionStatusPicture.Size = new System.Drawing.Size(16, 16);
        	this.connectionStatusPicture.TabIndex = 11;
        	this.connectionStatusPicture.TabStop = false;
        	//
        	// splitContainer
        	//
        	this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.splitContainer.Location = new System.Drawing.Point(0, 0);
        	this.splitContainer.Name = "splitContainer";
        	//
        	// splitContainer.Panel1
        	//
        	this.splitContainer.Panel1.Controls.Add(this.TabControl);
        	this.splitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1Paint);
        	//
        	// splitContainer.Panel2
        	//
        	this.splitContainer.Panel2.Controls.Add(this.ChatHistory);
        	this.splitContainer.Panel2.Controls.Add(this.connectionStopWaitingButton);
        	this.splitContainer.Panel2.Controls.Add(this.ChatText);
        	this.splitContainer.Panel2.Controls.Add(this.connectionStatusPicture);
        	this.splitContainer.Panel2.Controls.Add(this.connectionStatusLabel);
        	this.splitContainer.Size = new System.Drawing.Size(682, 504);
        	this.splitContainer.SplitterDistance = 503;
        	this.splitContainer.TabIndex = 13;
        	//
        	// TabControl
        	//
        	this.TabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
        	this.TabControl.Controls.Add(this.tabPage3);
        	this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.TabControl.Location = new System.Drawing.Point(0, 0);
        	this.TabControl.Name = "TabControl";
        	this.TabControl.SelectedIndex = 0;
        	this.TabControl.Size = new System.Drawing.Size(503, 504);
        	this.TabControl.TabIndex = 10;
        	//
        	// tabPage3
        	//
        	this.tabPage3.Controls.Add(this.SourceCode);
        	this.tabPage3.Location = new System.Drawing.Point(4, 4);
        	this.tabPage3.Name = "tabPage3";
        	this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage3.Size = new System.Drawing.Size(495, 478);
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
        	this.SourceCode.Size = new System.Drawing.Size(489, 472);
        	this.SourceCode.TabIndex = 1;
        	//
        	// SessionViewForm
        	//
        	this.Controls.Add(this.splitContainer);
        	this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.Name = "SessionViewForm";
        	this.Size = new System.Drawing.Size(682, 504);
        	this.Load += new System.EventHandler(this.SessionViewFormLoad);
        	((System.ComponentModel.ISupportInitialize)(this.connectionStatusPicture)).EndInit();
        	this.splitContainer.Panel1.ResumeLayout(false);
        	this.splitContainer.Panel2.ResumeLayout(false);
        	this.splitContainer.Panel2.PerformLayout();
        	this.splitContainer.ResumeLayout(false);
        	this.TabControl.ResumeLayout(false);
        	this.tabPage3.ResumeLayout(false);
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.SplitContainer splitContainer;

        private System.Windows.Forms.Button connectionStopWaitingButton;
        private System.Windows.Forms.PictureBox connectionStatusPicture;
        private System.Windows.Forms.Label connectionStatusLabel;
        private ClosableTabControl TabControl;
        private System.Windows.Forms.TextBox ChatText;
        private System.Windows.Forms.TextBox ChatHistory;
        internal ICSharpCode.TextEditor.TextEditorControl SourceCode;
        private System.Windows.Forms.TabPage tabPage3;
    }
}