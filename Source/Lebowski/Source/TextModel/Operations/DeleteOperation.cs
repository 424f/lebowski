namespace Lebowski.TextModel.Operations
{   
    public class DeleteOperation : TextOperation
    {
        public int Position { get; set; }

        public DeleteOperation(int position)
        {
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
}