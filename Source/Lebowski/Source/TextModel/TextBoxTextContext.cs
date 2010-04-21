using System;
using System.Windows.Forms;

namespace Lebowski.TextModel
{
	public class TextBoxTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		
		public event EventHandler<DeleteEventArgs> Deleted;
		
		public event EventHandler<ChangeEventArgs> Changed;
		
		public string Data
		{
			get { return TextBox.Text; }
			set { TextBox.Text = value; }
		}
		
		public void Insert(object issuer, InsertOperation operation)
		{
			throw new NotImplementedException();
		}
		
		public void Delete(object issuer, DeleteOperation operation)
		{
			throw new NotImplementedException();
		}
		
		TextBox TextBox;
		
		public TextBoxTextContext(TextBox textBox)
		{
			TextBox = textBox;
			TextBox.TextChanged += delegate(object sender, EventArgs e) { 
				OnChanged(new ChangeEventArgs(this));
			};
			
			// TODO: implement insert / delete
		}
		
		protected virtual void OnInserted(InsertEventArgs e)
		{
			if (Inserted != null) {
				Inserted(this, e);
			}
		}
		
		protected virtual void OnDeleted(DeleteEventArgs e)
		{
			if (Deleted != null) {
				Deleted(this, e);
			}
		}
		
		protected virtual void OnChanged(ChangeEventArgs e)
		{
			if (Changed != null) {
				Changed(this, e);
			}
		}
		
	}
}
