
namespace Lebowski.TextModel.Operations
{
    using System;
    /// <remarks>
    /// This class encapsulates a insert operation.
    /// A insert operation is defined by a text to insert and a position at which the text is to be inserted.
    /// </remarks>
    public class InsertOperation : TextOperation
    {
        /// <summary>
        /// Initializes a new instance of a InsertOperation.
        /// </summary>
        /// <param name="text"><see cref="string"></see></param>
        /// <param name="position"><see cref="int"></see></param>
        public InsertOperation(string text, int position)
        {
            Text = text;
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of a InsertOperation that inserts a
        /// single character at a specified position.
        /// </summary>
        /// <param name="c"><see cref="char"></see></param>
        /// <param name="position"><see cref="int"></see></param>
        public InsertOperation(char c, int position) : this(Convert.ToString(c), position) {}

        /// <value>
        /// The text to be inserted.
        /// </value>
        public string Text { get; set; }

        /// <value>
        /// The position at which the text should be inserted.
        /// </value>
        public int Position    { get; set; }        
        
        /// <inheritdoc />
        public override T Accept<T>(ITextOperationVisitor<T> visitor)
        {
            return visitor.VisitInsertOperation(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.Format("insert({0}, {1})", Text, Position);
        }
    }
}