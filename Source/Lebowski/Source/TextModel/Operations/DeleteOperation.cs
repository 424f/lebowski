namespace Lebowski.TextModel.Operations
{
    /// <remarks>
    /// This class encapsulates a insert operation.
    /// A insert operation is defined by a text to insert and a position at which the text is to be inserted.
    /// </remarks>
    public class DeleteOperation : TextOperation
    {
        /// <summary>
        ///  Position where the deletion should take place
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
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