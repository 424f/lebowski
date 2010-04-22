﻿using System;
using System.Windows.Forms;

namespace Lebowski.TextModel
{
	public class TextBoxTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		
		public event EventHandler<DeleteEventArgs> Deleted;
		
		public event EventHandler<ChangeEventArgs> Changed;
		
		public int SelectionStart
		{
			get { return TextBox.SelectionStart;	}
		}
		
		public int SelectionEnd
		{
			get { return TextBox.SelectionStart + TextBox.SelectionLength; }
		}
		
		public string Data
		{
			get { return TextBox.Text; }
			set { TextBox.Text = value; }
		}
		
		public void Insert(object issuer, InsertOperation operation)
		{
			TextBox.Text.Insert(operation.Position, operation.Text.ToString());
		}
		
		public void Delete(object issuer, DeleteOperation operation)
		{
			TextBox.Text.Remove(operation.Position, 1);
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
		
		public void SetSelection(int start, int last)
		{			
			TextBox.SelectionStart = start;
			TextBox.SelectionLength = last - start;
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
