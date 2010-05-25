
namespace Lebowski.TextModel.Operations
{
    using System;
    abstract public class TextOperation
    {
        public TextOperation()
        {
        }

        public ITextOperationVisitor<TextOperation> Transformer { get; protected set; }

        public TextOperation Transform(TextOperation other)
        {
            TextOperation textOperation = (TextOperation)other;
            return textOperation.Accept<TextOperation>(Transformer);
        }

        abstract public ReturnType Accept<ReturnType>(ITextOperationVisitor<ReturnType> operation);
    }
}