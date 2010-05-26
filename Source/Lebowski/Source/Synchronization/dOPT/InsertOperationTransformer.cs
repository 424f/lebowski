using System;

namespace Lebowski.Synchronization.dOPT
{
    using Lebowski.TextModel.Operations;
    
    class InsertOperationTransformer : ITextOperationVisitor<TextOperation>
    {
        private InsertOperation Operation { get; set; }

        public InsertOperationTransformer(InsertOperation op)
        {
            Operation = op;
        }

        public TextOperation VisitInsertOperation(InsertOperation other)
        {
            if (Operation.Position < other.Position)
            {
                return Operation;
            }
            else if (Operation.Position > other.Position)
            {
                return new InsertOperation(Operation.Text, Operation.Position+1);
            }
            else
            {
                if (Operation.Text == other.Text)
                {
                    return null;
                }
                else
                {
                    /* TODO:
                    if (Operation.Priority > other.Priority)
                    {
                        return new InsertOperation(Operation.Character, Operation.Position+1);
                    }
                    else
                    {
                        return new InsertOperation(Operation.Character, Operation.Position);
                    }
                    */
                    throw new NotImplementedException();
                }
            }
        }

        public TextOperation VisitDeleteOperation(DeleteOperation other)
        {
            if (Operation.Position < other.Position)
            {
                return new InsertOperation(Operation.Text, Operation.Position);
            }
            else
            {
                return new InsertOperation(Operation.Text, Operation.Position-1);
            }
        }
    }
}
