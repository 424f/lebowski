namespace Lebowski.Synchronization.dOPT
{
    using System;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// Transforms a DeleteOperation relative to another TextOperation,
    /// as required in the dOPT algorithm.
    /// </summary>    
    class DeleteOperationTransformer : ITextOperationVisitor<TextOperation>
    {
        /// <summary>
        /// Initializes a new instance of the DeleteOperationTransformer.
        /// </summary>
        /// <param name="operation">See <see cref="Operation">Operation</see>.</param>        
        public DeleteOperationTransformer(DeleteOperation operation)
        {
            Operation = operation;
        }

        /// <summary>
        /// The operation to be transformed.
        /// </summary>        
        public DeleteOperation Operation { get; private set; }        
        
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
            else
            {
                return new DeleteOperation(Operation.Position+1);
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
