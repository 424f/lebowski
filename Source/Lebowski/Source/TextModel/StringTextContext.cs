using System;

namespace Lebowski.TextModel
{
	public class StringTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		public event EventHandler<DeleteEventArgs> Deleted;
		public event EventHandler<ChangeEventArgs> Changed;
		
		public string Data { get; set; }
		public int SelectionStart { get; protected set; }
		public int SelectionEnd { get; protected set; }
		
		public StringTextContext()
		{
			Data = "";
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
		
		
		public virtual void Insert(object issuer, InsertOperation operation)
		{
			Data = Data.Substring(0, operation.Position) + operation.Text + Data.Substring(operation.Position);
			if(Inserted != null)
			{
				Inserted(this, new InsertEventArgs(issuer, operation));
			}
			if(Changed != null)
			{
				Changed(this, new ChangeEventArgs(issuer));
			}
		}
		
		public virtual void Delete(object issuer, DeleteOperation operation)
		{
			Data = Data.Substring(0, operation.Position) + Data.Substring(operation.Position+1);
			if(Deleted != null)
			{
				Deleted(this, new DeleteEventArgs(issuer, operation));
			}
			if(Changed != null)
			{
				Changed(this, new ChangeEventArgs(issuer));				
			}
		}
		
		public void SetSelection(int start, int last)
		{
			SelectionStart = start;
			SelectionEnd = last;
		}		
		
		public void Invoke(Action d)
		{
			d();
		}
	}
}
