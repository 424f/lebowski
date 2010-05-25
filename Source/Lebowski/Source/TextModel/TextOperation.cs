using System;

namespace Lebowski.TextModel
{
    abstract public class TextOperation
    {        
        public ITextOperationVisitor<TextOperation> Transformer { get; protected set; }
        
        
        public TextOperation()
        {
        }

        public TextOperation Transform(TextOperation other)
        {
            TextOperation textOperation = (TextOperation)other;
            return textOperation.Accept<TextOperation>(Transformer);
        }        
        
        abstract public ReturnType Accept<ReturnType>(ITextOperationVisitor<ReturnType> operation);
    }
}
