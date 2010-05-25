/*
 * Created by SharpDevelop.
 * User: bo
 * Date: 30.04.2010
 * Time: 15:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lebowski.Net.Tcp
{
    partial class TcpShareForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.portText = new System.Windows.Forms.TextBox();
            this.shareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = "To share a document via TCP, you must forward the port you select below in your f" +
            "irewall configuration. Otherwise, your partner will not be able to join your ses" +
            "sion.";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // portText
            // 
            this.portText.Location = new System.Drawing.Point(13, 110);
            this.portText.Name = "portText";
            this.portText.Size = new System.Drawing.Size(306, 23);
            this.portText.TabIndex = 2;
            // 
            // shareButton
            // 
            this.shareButton.Location = new System.Drawing.Point(117, 152);
            this.shareButton.Name = "shareButton";
            this.shareButton.Size = new System.Drawing.Size(75, 23);
            this.shareButton.TabIndex = 3;
            this.shareButton.Text = "Share";
            this.shareButton.UseVisualStyleBackColor = true;
            this.shareButton.Click += new System.EventHandler(this.ShareButtonClick);
            // 
            // TcpShareForm
            // 
            this.AcceptButton = this.shareButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 185);
            this.Controls.Add(this.shareButton);
            this.Controls.Add(this.portText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TcpShareForm";
            this.Text = "Share";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Button shareButton;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
