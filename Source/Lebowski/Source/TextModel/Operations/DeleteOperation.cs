namespace Lebowski.TextModel.Operations
{
    /// <summary>
    /// This class encapsulates a delete operation.
    /// A delete operation is defined by a position only, as only one character is deleted at a time.
    /// </summary>
    public class DeleteOperation : TextOperation
    {
        public int Position { get; set; }

        /// <summary>
        /// Initializes a new instance of a InsertOperation.
        /// </summary>
        /// <param name="position"><see cref="int"></see></param>
        public DeleteOperation(int position)
        {
            Transformer = new DeleteOperationTransformer(this);
            Position = position;
        }

        public override T Accept<T>(ITextOperationVisitor<T> visitor)
        {
            return visitor.VisitDeleteOperation(this);
        }

        public override string ToString()
        {
            return string.Format("delete({0})", Position);
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