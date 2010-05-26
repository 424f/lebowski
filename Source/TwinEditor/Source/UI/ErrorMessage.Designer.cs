
namespace TwinEditor.UI
{
    partial class ErrorMessage
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorMessage));
        	this.tabControl = new System.Windows.Forms.TabControl();
        	this.ErrorTab = new System.Windows.Forms.TabPage();
        	this.messageLabel = new System.Windows.Forms.Label();
        	this.reportButton = new System.Windows.Forms.Button();
        	this.okButton = new System.Windows.Forms.Button();
        	this.titleLabel = new System.Windows.Forms.Label();
        	this.errorPicture = new System.Windows.Forms.PictureBox();
        	this.ExceptionTab = new System.Windows.Forms.TabPage();
        	this.exceptionText = new System.Windows.Forms.TextBox();
        	this.tabControl.SuspendLayout();
        	this.ErrorTab.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.errorPicture)).BeginInit();
        	this.ExceptionTab.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// tabControl
        	// 
        	this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
        	this.tabControl.Controls.Add(this.ErrorTab);
        	this.tabControl.Controls.Add(this.ExceptionTab);
        	this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.tabControl.Location = new System.Drawing.Point(0, 0);
        	this.tabControl.Name = "tabControl";
        	this.tabControl.SelectedIndex = 0;
        	this.tabControl.Size = new System.Drawing.Size(483, 348);
        	this.tabControl.TabIndex = 0;
        	// 
        	// ErrorTab
        	// 
        	this.ErrorTab.Controls.Add(this.messageLabel);
        	this.ErrorTab.Controls.Add(this.reportButton);
        	this.ErrorTab.Controls.Add(this.okButton);
        	this.ErrorTab.Controls.Add(this.titleLabel);
        	this.ErrorTab.Controls.Add(this.errorPicture);
        	this.ErrorTab.Location = new System.Drawing.Point(4, 4);
        	this.ErrorTab.Name = "ErrorTab";
        	this.ErrorTab.Padding = new System.Windows.Forms.Padding(3);
        	this.ErrorTab.Size = new System.Drawing.Size(475, 322);
        	this.ErrorTab.TabIndex = 0;
        	this.ErrorTab.Text = "Error";
        	this.ErrorTab.UseVisualStyleBackColor = true;
        	// 
        	// messageLabel
        	// 
        	this.messageLabel.Location = new System.Drawing.Point(8, 47);
        	this.messageLabel.Name = "messageLabel";
        	this.messageLabel.Size = new System.Drawing.Size(459, 236);
        	this.messageLabel.TabIndex = 2;
        	this.messageLabel.Text = "label2";
        	// 
        	// reportButton
        	// 
        	this.reportButton.Enabled = false;
        	this.reportButton.Location = new System.Drawing.Point(271, 285);
        	this.reportButton.Name = "reportButton";
        	this.reportButton.Size = new System.Drawing.Size(95, 31);
        	this.reportButton.TabIndex = 1;
        	this.reportButton.Text = "Report bug";
        	this.reportButton.UseVisualStyleBackColor = true;
        	this.reportButton.Click += new System.EventHandler(this.ReportButtonClick);
        	// 
        	// okButton
        	// 
        	this.okButton.Location = new System.Drawing.Point(372, 286);
        	this.okButton.Name = "okButton";
        	this.okButton.Size = new System.Drawing.Size(95, 31);
        	this.okButton.TabIndex = 1;
        	this.okButton.Text = "OK";
        	this.okButton.UseVisualStyleBackColor = true;
        	this.okButton.Click += new System.EventHandler(this.OkButtonClick);
        	// 
        	// titleLabel
        	// 
        	this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        	this.titleLabel.Location = new System.Drawing.Point(49, 10);
        	this.titleLabel.Name = "titleLabel";
        	this.titleLabel.Size = new System.Drawing.Size(418, 33);
        	this.titleLabel.TabIndex = 1;
        	this.titleLabel.Text = "Error title";
        	// 
        	// errorPicture
        	// 
        	this.errorPicture.Image = ((System.Drawing.Image)(resources.GetObject("errorPicture.Image")));
        	this.errorPicture.Location = new System.Drawing.Point(8, 8);
        	this.errorPicture.Name = "errorPicture";
        	this.errorPicture.Size = new System.Drawing.Size(34, 35);
        	this.errorPicture.TabIndex = 0;
        	this.errorPicture.TabStop = false;
        	// 
        	// ExceptionTab
        	// 
        	this.ExceptionTab.Controls.Add(this.exceptionText);
        	this.ExceptionTab.Location = new System.Drawing.Point(4, 4);
        	this.ExceptionTab.Name = "ExceptionTab";
        	this.ExceptionTab.Padding = new System.Windows.Forms.Padding(3);
        	this.ExceptionTab.Size = new System.Drawing.Size(475, 322);
        	this.ExceptionTab.TabIndex = 1;
        	this.ExceptionTab.Text = "Exception";
        	this.ExceptionTab.UseVisualStyleBackColor = true;
        	// 
        	// exceptionText
        	// 
        	this.exceptionText.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	this.exceptionText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.exceptionText.Location = new System.Drawing.Point(3, 3);
        	this.exceptionText.Multiline = true;
        	this.exceptionText.Name = "exceptionText";
        	this.exceptionText.Size = new System.Drawing.Size(469, 316);
        	this.exceptionText.TabIndex = 0;
        	// 
        	// ErrorMessage
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.White;
        	this.ClientSize = new System.Drawing.Size(483, 348);
        	this.Controls.Add(this.tabControl);
        	this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "ErrorMessage";
        	this.Text = "ErrorMessage";
        	this.Load += new System.EventHandler(this.ErrorMessageLoad);
        	this.tabControl.ResumeLayout(false);
        	this.ErrorTab.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.errorPicture)).EndInit();
        	this.ExceptionTab.ResumeLayout(false);
        	this.ExceptionTab.PerformLayout();
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox exceptionText;
        private System.Windows.Forms.PictureBox errorPicture;
        private System.Windows.Forms.TabPage ExceptionTab;
        private System.Windows.Forms.TabPage ErrorTab;
        private System.Windows.Forms.TabControl tabControl;
    }
}