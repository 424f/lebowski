namespace Lebowski.TextModel.Operations
{
    /// <remarks>
    /// This class encapsulates a delete operation.
    /// A insert operation is defined by the position of the character to be deleted.
    /// </remarks>
    public class DeleteOperation : TextOperation
    {
        /// <summary>
        ///  The position of the character that should be deleted
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">The position of the character to be deleted</param>
        public DeleteOperation(int position)
        {
            Position = position;
        }

        /// <summary>
        /// Accepts
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
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