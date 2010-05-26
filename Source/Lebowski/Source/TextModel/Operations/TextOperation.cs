namespace Lebowski.TextModel.Operations
{
    using System;
    
    /// <summary>
    /// A text operation, altering a TextContext in some way.
    /// </summary>
    abstract public class TextOperation
    {
        /// <summary>
        /// Accepts a visitor implementing the ITextOperationVisitor&lt;T&gt; interface
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The result returned from the visitor.</returns>        
        abstract public ReturnType Accept<ReturnType>(ITextOperationVisitor<ReturnType> visitor);
    }
}