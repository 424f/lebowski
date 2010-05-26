using System;

namespace Lebowski.Synchronization.dOPT
{
    using Lebowski.TextModel.Operations;
    
    class DeleteOperationTransformer : ITextOperationVisitor<TextOperation>
    {
        DeleteOperation Operation;

        public DeleteOperationTransformer(DeleteOperation operation)
        {
            Operation = operation;
        }

        public TextOperation VisitInsertOperation(InsertOperation other)
        {
            if (Operation.Position < other.Position)
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
            if (Operation.Position < other.Position)
            {
                return Operation;
            }
            else if (Operation.Position > other.Position)
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
