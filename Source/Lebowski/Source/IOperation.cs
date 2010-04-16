using System;

namespace Lebowski
{
	public interface IOperation<ContextType>
	{		
		/// <summary>
		/// Applies the operation
		/// </summary>
		/// <param name="context">Context the operation should be applied to</param>
		void Apply(ContextType context);

		/// <summary>
		/// Transforms the operation by a previously performed operation
		/// </summary>
		/// <param name="other">Previous operation</param>		
		IOperation<ContextType> Transform(IOperation<ContextType> other);
	}
}
