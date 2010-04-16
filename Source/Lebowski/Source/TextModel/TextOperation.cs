using System;

namespace Lebowski.TextModel
{
	abstract public class TextOperation : IOperation<ITextContext>
	{		
		public ITextOperationVisitor<TextOperation> Transformer { get; protected set; }
		
		
		public TextOperation()
		{
		}

		public IOperation<ITextContext> Transform(IOperation<ITextContext> other)
		{
			TextOperation textOperation = (TextOperation)other;
			return textOperation.Accept<TextOperation>(Transformer);
		}		
		
		abstract public ReturnType Accept<ReturnType>(ITextOperationVisitor<ReturnType> operation);
		abstract public void Apply(ITextContext context);
	}
}
