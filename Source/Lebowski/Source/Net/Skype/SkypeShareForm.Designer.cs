
namespace Lebowski.Net.Skype
{
	partial class SkypeShareForm
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
			this.shareButton = new System.Windows.Forms.Button();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.IconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.UsernameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// shareButton
			// 
			this.shareButton.Location = new System.Drawing.Point(68, 227);
			this.shareButton.Name = "shareButton";
			this.shareButton.Size = new System.Drawing.Size(140, 23);
			this.shareButton.TabIndex = 0;
			this.shareButton.Text = "Share document";
			this.shareButton.UseVisualStyleBackColor = true;
			this.shareButton.Click += new System.EventHandler(this.ShareButtonClick);
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToResizeColumns = false;
			this.dataGridView.AllowUserToResizeRows = false;
			this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.IconColumn,
									this.UsernameColumn});
			this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridView.Location = new System.Drawing.Point(13, 13);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.RowHeadersVisible = false;
			this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView.Size = new System.Drawing.Size(259, 208);
			this.dataGridView.TabIndex = 1;
			// 
			// IconColumn
			// 
			this.IconColumn.FillWeight = 32F;
			this.IconColumn.HeaderText = "";
			this.IconColumn.Name = "IconColumn";
			this.IconColumn.ReadOnly = true;
			this.IconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.IconColumn.Width = 32;
			// 
			// UsernameColumn
			// 
			this.UsernameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.UsernameColumn.HeaderText = "User";
			this.UsernameColumn.Name = "UsernameColumn";
			this.UsernameColumn.ReadOnly = true;
			this.UsernameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// SkypeShareForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.shareButton);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SkypeShareForm";
			this.ShowInTaskbar = false;
			this.Text = "Share";
			this.Load += new System.EventHandler(this.SkypeShareFormLoad);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn UsernameColumn;
		private System.Windows.Forms.DataGridViewImageColumn IconColumn;
		private System.Windows.Forms.Button shareButton;
	}
}
