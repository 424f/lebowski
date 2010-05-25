namespace Lebowski.TextModel.Operations
{
    public interface ITextOperationVisitor<ReturnType>
    {
        ReturnType VisitInsertOperation(InsertOperation textOperation);
        ReturnType VisitDeleteOperation(DeleteOperation deleteOperation);
    }
}