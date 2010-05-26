
namespace Lebowski.TextModel.Operations
{
    using System;
    public class InsertOperation : TextOperation
    {
        public string Text { get; set; }

        public int Position    { get; set; }

        public InsertOperation(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public InsertOperation(char c, int position) : this(Convert.ToString(c), position)
        {

        }

        public override T Accept<T>(ITextOperationVisitor<T> visitor)
        {
            return visitor.VisitInsertOperation(this);
        }

        public override string ToString()
        {
            return String.Format("insert({0}, {1})", Text, Position);
        }
    }
}