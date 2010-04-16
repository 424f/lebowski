using System;

namespace Lebowski.TextModel
{
	public class StringTextContext : ITextContext
	{
		public event EventHandler<InsertEventArgs> Inserted;
		public event EventHandler<DeleteEventArgs> Deleted;
		public event EventHandler<EventArgs> Changed;
		
		public string Data { get; set; }
		
		public StringTextContext()
		{
			Data = "";
		}
		
		public void Insert(InsertOperation operation, bool alreadyPerformed)
		{
			if(!alreadyPerformed)
			{
				Data = Data.Substring(0, operation.Position) + operation.Character + Data.Substring(operation.Position);
			}
			if(Inserted != null)
			{
				Inserted(this, new InsertEventArgs(operation));
			}
			if(Changed != null)
			{
				Changed(this, null);
			}
		}
		
		public void Delete(DeleteOperation operation, bool alreadyPerformed)
		{
			if(!alreadyPerformed)
			{
				Data = Data.Substring(0, operation.Position) + Data.Substring(operation.Position+1);
			}
			if(Deleted != null)
			{
				Deleted(this, new DeleteEventArgs(operation));
			}
			if(Changed != null)
			{
				Changed(this, null);				
			}
		}
	}
}
