namespace Lebowski.Synchronization.dOPT
{
    using System;
    using Lebowski.TextModel.Operations;
    
    /// <summary>
    /// Transforms an InsertOperation relative to another TextOperation,
    /// as required in the dOPT algorithm.
    /// </summary>
    class InsertOperationTransformer : ITextOperationVisitor<TextOperation>
    {
        /// <summary>
        /// Initializes a new instance of the InsertOperationTransformer.
        /// </summary>
        /// <param name="op">See <see cref="Operation">Operation</see>.</param>        
        public InsertOperationTransformer(InsertOperation op)
        {
            Operation = op;
        }
        
        /// <summary>
        /// The operation to be transformed.
        /// </summary>
        public InsertOperation Operation { get; private set; }        

        /// <summary>
        /// Transforms <see cref="Operation">Operation</see> given
        /// an InsertOperation that previously occurred.
        /// </summary>
        /// <param name="other">The operation that was previously applied.</param>
        /// <returns>The transformed operation</returns>       
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

        /// <summary>
        /// Transforms <see cref="Operation">Operation</see> given
        /// a DeleteOperation that previously occurred.
        /// </summary>
        /// <param name="other">The operation that was previously applied.</param>
        /// <returns>The transformed operation</returns>   
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
