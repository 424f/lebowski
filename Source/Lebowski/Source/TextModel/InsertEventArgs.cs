namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;
    
    /// <summary>
    /// Provides data for the <see cref="ITextContext.Insert">Insert</see> event.
    /// This is used for algorithms that are based on atomic operations (e.g.
    /// operational transformations).
    /// </summary>
    public class InsertEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the InsertEventArgs with a
        /// InsertOperation that has been performed.
        /// </summary>
        /// <param name="issuer">See <see cref="Issuer">Issuer</see>.</param>
        /// <param name="operation">See <see cref="Operation">Operation</see></param>
        public InsertEventArgs(object issuer, InsertOperation operation)
        {
            Operation = operation;
            Issuer = issuer;
        }
        
        /// <summary>
        /// The object that issued this command.
        /// </summary>
        public object Issuer { get; protected set; }        
        
        /// <summary>
        /// The operation that has been performed.
        /// </summary>
        public InsertOperation Operation { get; protected set; }
    }
}
