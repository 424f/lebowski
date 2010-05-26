namespace Lebowski.TextModel
{
    using System;    
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// Provides data for the <see cref="ITextContext.Delete">Delete</see> event.
    /// This is used for algorithms that are based on atomic operations (e.g.
    /// operational transformations).
    /// </summary>        
    public class DeleteEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the DeleteEventArgs with a
        /// DeleteOperation that has been performed.
        /// </summary>
        /// <param name="issuer">The issuer of the operation.</param>
        /// <param name="operation">See <see cref="Operation">Operation</see></param>
        public DeleteEventArgs(object issuer, DeleteOperation operation)
        {
            Operation = operation;
            Issuer = issuer;
        }
        
        /// <summary>
        /// The operation that has been performed.
        /// </summary>
        public DeleteOperation Operation { get; protected set; }
        
        /// <summary>
        /// The object that issued this command.
        /// </summary>
        public object Issuer { get; protected set; }
        
    }
}
