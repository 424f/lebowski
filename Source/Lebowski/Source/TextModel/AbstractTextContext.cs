﻿
using System;

namespace Lebowski.TextModel
{
	public abstract class AbstractTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		public event EventHandler<DeleteEventArgs> Deleted;
		public event EventHandler<ChangeEventArgs> Changed;
		
		public string SelectedText
		{
			get 
			{
				return Data.Substring(SelectionStart, SelectionEnd-SelectionStart);
			}
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
		
		public virtual int CaretPosition { get; set; }
		
		public abstract string Data { get; set;	}
		public abstract int SelectionStart { get; protected set;}		
		public abstract int SelectionEnd { get; protected set; }		
		public abstract void Insert(object issuer, InsertOperation operation);		
		public abstract void Delete(object issuer, DeleteOperation operation);		
		public abstract void SetSelection(int start, int last);
		public abstract void Invoke(Action action);
	}
}
