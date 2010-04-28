using System;
using ICSharpCode.TextEditor;

// TODO: extract common functionality with TextBoxTextContext into common abstract superclass

namespace Lebowski.TextModel
{
	public class TextEditorTextContext : AbstractTextContext
	{
		public override int SelectionStart
		{
			get
			{ 
				if(!TextBox.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
					return 0;
				return TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
			}
			protected set { throw new NotImplementedException(); }
		}
		
		public override int SelectionEnd
		{
			get
			{
				if(!TextBox.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
					return 0;				
				return SelectionStart + TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length;
			}
			protected set { throw new NotImplementedException(); }
		}
		
		public override int CaretPosition
		{
			get 
			{
				return TextBox.ActiveTextAreaControl.Caret.Offset;
			}
			
			set
			{
				TextBox.ActiveTextAreaControl.Caret.Position = TextBox.Document.OffsetToPosition(value);
			}
		}
		
		public override string Data
		{
			get { return TextBox.Text; }
			set
			{
				TextBox.TextChanged -= TextBoxChanged;
				TextBox.Text = value;
				TextBox.Refresh();
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
		
		public override void SetSelection(int start, int last)
		{	
			TextLocation startLocation = TextBox.Document.OffsetToPosition(start);
			TextLocation lastLocation = TextBox.Document.OffsetToPosition(last);
			TextBox.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, lastLocation);
		}			
		
		public override bool HasSelection
		{
			get
			{
				return TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0 &&
					     SelectionStart != SelectionEnd;
			}
		}		
		
		public override void Invoke(Action d) 
		{
			TextBox.BeginInvoke(d);
		}
		
	}
}
