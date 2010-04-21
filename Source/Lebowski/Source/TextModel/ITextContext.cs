using System;

namespace Lebowski.TextModel
{
	public interface ITextContext
	{
		string Data { get; set; }
		
		void Insert(object issuer, InsertOperation operation);
		void Delete(object issuer, DeleteOperation operation);
		
		event EventHandler<InsertEventArgs> Inserted;
		event EventHandler<DeleteEventArgs> Deleted;
		event EventHandler<ChangeEventArgs> Changed;
	}
	
	public class InsertEventArgs : EventArgs
	{
		public InsertOperation Operation { get; protected set; }
		public object Issuer { get; protected set; }
		
		public InsertEventArgs(object issuer, InsertOperation operation)
		{
			Operation = operation;
			Issuer = issuer;
		}
	}
	
	public class DeleteEventArgs : EventArgs
	{
		public DeleteOperation Operation { get; protected set; }
		public object Issuer { get; protected set; }
		
		public DeleteEventArgs(object issuer, DeleteOperation operation)
		{
			Operation = operation;
			Issuer = issuer;
		}
	}	
	
	public class ChangeEventArgs : EventArgs
	{
		public object Issuer { get; protected set; }
		
		public ChangeEventArgs(object issuer)
		{
			Issuer = issuer;
		}
	}
}
