/*
 * Created by SharpDevelop.
 * User: bo
 * Date: 01.05.2010
 * Time: 00:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TwinEditor
{
	partial class ExecutionTabControl
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
			this.StandardOutput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// StandardOutput
			// 
			this.StandardOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.StandardOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StandardOutput.Location = new System.Drawing.Point(4, 4);
			this.StandardOutput.Multiline = true;
			this.StandardOutput.Name = "StandardOutput";
			this.StandardOutput.Size = new System.Drawing.Size(524, 339);
			this.StandardOutput.TabIndex = 0;
			// 
			// ExecutionTabControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StandardOutput);
			this.Name = "ExecutionTabControl";
			this.Size = new System.Drawing.Size(531, 369);
			this.Load += new System.EventHandler(this.ExecutionTabControlLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox StandardOutput;
	}
}
