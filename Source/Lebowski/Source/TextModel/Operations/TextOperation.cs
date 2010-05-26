namespace Lebowski.TextModel.Operations
{
    using System;
    abstract public class TextOperation
    {
        public TextOperation()
        {
        }

        abstract public ReturnType Accept<ReturnType>(ITextOperationVisitor<ReturnType> operation);
    }
}