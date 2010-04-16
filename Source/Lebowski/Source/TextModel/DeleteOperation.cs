﻿using System;

namespace Lebowski.TextModel
{
	public class DeleteOperation : TextOperation
	{
		public int Position { get; set; }
		
		public DeleteOperation(int position)
		{			
			Transformer = new DeleteOperationTransformer(this);
			Position = position;
		}
		
		public override void Apply(ITextContext context)
		{
			context.Delete(Position, 1);
		}
		
		public override T Accept<T>(ITextOperationVisitor<T> visitor)
		{
			return visitor.VisitDeleteOperation(this);
		}
		
		public override string ToString()
		{
			return String.Format("delete({0})", Position);
		}
	}
	
	class DeleteOperationTransformer : ITextOperationVisitor<TextOperation>
	{
        DeleteOperation Operation;     
        
        public DeleteOperationTransformer(DeleteOperation operation)
        {
        	Operation = operation;
        }
		
		public TextOperation VisitInsertOperation(InsertOperation other)
		{
			if(Operation.Position < other.Position)
			{
				return Operation;
			}
			else
			{
				return new DeleteOperation(Operation.Position+1);
			}
		}
		
		public TextOperation VisitDeleteOperation(DeleteOperation other)
		{
			if(Operation.Position < other.Position)
			{
				return Operation;
			}
			else if(Operation.Position > other.Position)
			{
				return new DeleteOperation(Operation.Position-1);
			}
			else
			{
				return null;
			}
		}		
	}	
}