
namespace Lebowski.TextModel.Operations
{
    using System;
    /// <summary>
    /// This class encapsulates a insert operation.
    /// A insert operation is defined by a text to insert and a position at which the text is to be inserted.
    /// </summary>
    public class InsertOperation : TextOperation
    {
        /// <value>
        /// The text to be inserted.
        /// </value>
        public string Text { get; set; }

        /// <value>
        /// The position at which the text should be inserted.
        /// </value>
        public int Position    { get; set; }

        /// <summary>
        /// Initializes a new instance of a InsertOperation.
        /// </summary>
        /// <param name="text"><see cref="string"></see></param>
        /// <param name="position"><see cref="int"></see></param>
        public InsertOperation(string text, int position)
        {
            Transformer = new InsertOperationTransformer(this);
            Text = text;
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of a InsertOperation.
        /// </summary>
        /// <param name="text"><see cref="char"></see></param>
        /// <param name="position"><see cref="int"></see></param>
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

    class InsertOperationTransformer : ITextOperationVisitor<TextOperation>
    {
        private InsertOperation Operation { get; set; }

        public InsertOperationTransformer(InsertOperation op)
        {
            Operation = op;
        }
/*
def insert_insert(oi, oj, pi, pj):
    else:
        if oi.c == oj.c:
            return None
        else:
            if pi > pj:
                return Operation('insert', oi.pos+1, oi.c)
            else:
                return Operation('insert', oi.pos, oi.c)*/

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