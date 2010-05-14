
namespace TwinEditor
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
        	this.ExceptionTab = new System.Windows.Forms.TabPage();
        	this.button1 = new System.Windows.Forms.Button();
        	this.errorPicture = new System.Windows.Forms.PictureBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.exceptionText = new System.Windows.Forms.TextBox();
        	this.tabControl.SuspendLayout();
        	this.ErrorTab.SuspendLayout();
        	this.ExceptionTab.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.errorPicture)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// tabControl
        	// 
        	this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
        	this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl.Controls.Add(this.ErrorTab);
        	this.tabControl.Controls.Add(this.ExceptionTab);
        	this.tabControl.Location = new System.Drawing.Point(0, 0);
        	this.tabControl.Name = "tabControl";
        	this.tabControl.SelectedIndex = 0;
        	this.tabControl.Size = new System.Drawing.Size(485, 349);
        	this.tabControl.TabIndex = 0;
        	// 
        	// ErrorTab
        	// 
        	this.ErrorTab.Controls.Add(this.label2);
        	this.ErrorTab.Controls.Add(this.button1);
        	this.ErrorTab.Controls.Add(this.label1);
        	this.ErrorTab.Controls.Add(this.errorPicture);
        	this.ErrorTab.Location = new System.Drawing.Point(4, 4);
        	this.ErrorTab.Name = "ErrorTab";
        	this.ErrorTab.Padding = new System.Windows.Forms.Padding(3);
        	this.ErrorTab.Size = new System.Drawing.Size(477, 323);
        	this.ErrorTab.TabIndex = 0;
        	this.ErrorTab.Text = "Error";
        	this.ErrorTab.UseVisualStyleBackColor = true;
        	// 
        	// ExceptionTab
        	// 
        	this.ExceptionTab.Controls.Add(this.exceptionText);
        	this.ExceptionTab.Location = new System.Drawing.Point(4, 4);
        	this.ExceptionTab.Name = "ExceptionTab";
        	this.ExceptionTab.Padding = new System.Windows.Forms.Padding(3);
        	this.ExceptionTab.Size = new System.Drawing.Size(477, 323);
        	this.ExceptionTab.TabIndex = 1;
        	this.ExceptionTab.Text = "Exception";
        	this.ExceptionTab.UseVisualStyleBackColor = true;
        	// 
        	// button1
        	// 
        	this.button1.Location = new System.Drawing.Point(372, 286);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(95, 31);
        	this.button1.TabIndex = 1;
        	this.button1.Text = "OK";
        	this.button1.UseVisualStyleBackColor = true;
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
        	// label1
        	// 
        	this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        	this.label1.Location = new System.Drawing.Point(49, 10);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(418, 33);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Error title";
        	// 
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(8, 47);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(459, 236);
        	this.label2.TabIndex = 2;
        	this.label2.Text = "label2";
        	// 
        	// exceptionText
        	// 
        	this.exceptionText.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.exceptionText.Location = new System.Drawing.Point(3, 3);
        	this.exceptionText.Multiline = true;
        	this.exceptionText.Name = "exceptionText";
        	this.exceptionText.Size = new System.Drawing.Size(471, 317);
        	this.exceptionText.TabIndex = 0;
        	// 
        	// ErrorMessage
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(483, 348);
        	this.Controls.Add(this.tabControl);
        	this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "ErrorMessage";
        	this.Text = "ErrorMessage";
        	this.Load += new System.EventHandler(this.ErrorMessageLoad);
        	this.tabControl.ResumeLayout(false);
        	this.ErrorTab.ResumeLayout(false);
        	this.ExceptionTab.ResumeLayout(false);
        	this.ExceptionTab.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.errorPicture)).EndInit();
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.TextBox exceptionText;
        private System.Windows.Forms.PictureBox errorPicture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage ExceptionTab;
        private System.Windows.Forms.TabPage ErrorTab;
        private System.Windows.Forms.TabControl tabControl;
    }
}
