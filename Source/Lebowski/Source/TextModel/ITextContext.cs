using System;

namespace Lebowski.TextModel
{
	public interface ITextContext
	{
		string Data { get; set; }
		
		void Insert(InsertOperation operation, bool local);
		void Delete(DeleteOperation operation, bool local);
		
		event EventHandler<InsertEventArgs> Inserted;
		event EventHandler<DeleteEventArgs> Deleted;
		event EventHandler<EventArgs> Changed;
	}
	
	public class InsertEventArgs : EventArgs
	{
		public InsertOperation Operation { set; protected get; }
		
		public InsertEventArgs(InsertOperation operation)
		{
			Operation = operation;
		}
	}
	
	public class DeleteEventArgs : EventArgs
	{
		public DeleteOperation Operation { set; protected get; }
		
		public DeleteEventArgs(DeleteOperation operation)
		{
			Operation = operation;
		}
	}	
}
