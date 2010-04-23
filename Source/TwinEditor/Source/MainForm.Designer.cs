
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
			this.SuspendLayout();
			// 
			// SourceCode
			// 
			this.SourceCode.Location = new System.Drawing.Point(13, 13);
			this.SourceCode.Name = "SourceCode";
			this.SourceCode.Size = new System.Drawing.Size(445, 351);
			this.SourceCode.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 371);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(445, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Information";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 404);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SourceCode);
			this.Name = "MainForm";
			this.Text = "TwinEditor";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public System.Windows.Forms.Label label1;
		public ICSharpCode.TextEditor.TextEditorControl SourceCode;
	}
}
