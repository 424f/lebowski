using System;
using System.Windows.Forms;

namespace Lebowski.TextModel
{
	public class TextBoxTextContext : AbstractTextContext
	{		
		public override int SelectionStart
		{
			get { return TextBox.SelectionStart; }
			protected set { TextBox.SelectionStart = value; }
		}
		
		public override int SelectionEnd
		{
			get { return TextBox.SelectionStart + TextBox.SelectionLength; }
			protected set { TextBox.SelectionLength = value - TextBox.SelectionStart; }
		}
		
		public override string Data
		{
			get { return TextBox.Text; }
			set
			{
				TextBox.TextChanged -= TextBoxChanged;
				TextBox.Text = value;
				TextBox.TextChanged += TextBoxChanged;
			}
		}
		
		public override void Insert(object issuer, InsertOperation operation)
		{
			TextBox.TextChanged -= TextBoxChanged;
			TextBox.Text.Insert(operation.Position, operation.Text.ToString());
			TextBox.TextChanged += TextBoxChanged;
		}
		
		public override void Delete(object issuer, DeleteOperation operation)
		{
			TextBox.TextChanged -= TextBoxChanged;
			TextBox.Text.Remove(operation.Position, 1);
			TextBox.TextChanged += TextBoxChanged;
		}
		
		TextBox TextBox;
		
		public TextBoxTextContext(TextBox textBox)
		{
			TextBox = textBox;
			TextBox.TextChanged += TextBoxChanged;
			
			// TODO: implement insert / delete
		}
		
		public void TextBoxChanged(object sender, EventArgs e)
		{
			OnChanged(new ChangeEventArgs(this));
		}
		
		public override void SetSelection(int start, int last)
		{			
			TextBox.SelectionStart = start;
			TextBox.SelectionLength = last - start;
		}			
		
		public override void Invoke(Action d) 
		{
			TextBox.BeginInvoke(d);
		}
		
	}
}
