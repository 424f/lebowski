namespace Lebowski.TextModel.Operations
{
    /// <remarks>
    /// Represents a deletion of a single character at a specified position.
    /// A insert operation is defined by the position of the character to be deleted.
    /// </remarks>
    public class DeleteOperation : TextOperation
    {       
        /// <summary>
        /// Initializes a new instance of the DeleteOperation class, given the
        /// deletion position.
        /// </summary>
        /// <param name="position">The position of the character to be deleted.</param>
        public DeleteOperation(int position)
        {
            Position = position;
        }

        /// <summary>
        /// The position of the character that should be deleted
        /// </summary>
        public int Position { get; set; }        
        
        /// <inheritdoc />
        public override T Accept<T>(ITextOperationVisitor<T> visitor)
        {
            return visitor.VisitDeleteOperation(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("delete({0})", Position);
        }
    }
}