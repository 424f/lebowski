using System;
using ICSharpCode.TextEditor;

// TODO: extract common functionality with TextBoxTextContext into common abstract superclass

namespace Lebowski.TextModel
{
	public class TextEditorTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		
		public event EventHandler<DeleteEventArgs> Deleted;
		
		public event EventHandler<ChangeEventArgs> Changed;
		
		public int SelectionStart
		{
			get { return 0; } //TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;	}
		}
		
		public int SelectionEnd
		{
			get { return 0; } //SelectionStart + TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length; }
		}
		
		public string Data
		{
			get { return TextBox.Text; }
			set
			{
				TextBox.TextChanged -= TextBoxChanged;
				TextBox.Text = value;
				TextBox.TextChanged += TextBoxChanged;
			}
		}
		
		public void Insert(object issuer, InsertOperation operation)
		{
			TextBox.TextChanged -= TextBoxChanged;
			TextBox.Text.Insert(operation.Position, operation.Text.ToString());
			TextBox.TextChanged += TextBoxChanged;
		}
		
		public void Delete(object issuer, DeleteOperation operation)
		{
			TextBox.TextChanged -= TextBoxChanged;
			TextBox.Text.Remove(operation.Position, 1);
			TextBox.TextChanged += TextBoxChanged;
		}
		
		TextEditorControl TextBox;
		
		public TextEditorTextContext(TextEditorControl textBox)
		{
			TextBox = textBox;
			TextBox.TextChanged += TextBoxChanged;
			
			// TODO: implement insert / delete
		}
		
		public void TextBoxChanged(object sender, EventArgs e)
		{
			OnChanged(new ChangeEventArgs(this));
		}
		
		public void SetSelection(int start, int last)
		{			
			/*var selection = TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0];
			selection.Offset = start;
			selection.Length = last - start;
			TextBox.ActiveTextAreaControl.SelectionManager.SetSelection(selection);*/
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
		
		public void Invoke(Action d) 
		{
			TextBox.BeginInvoke(d);
		}
		
	}
}
