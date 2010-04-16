using System;

namespace Lebowski.TextModel
{
	public interface ITextOperationVisitor<ReturnType>
	{
		ReturnType VisitInsertOperation(InsertOperation textOperation);
		ReturnType VisitDeleteOperation(DeleteOperation deleteOperation);
	}
}
