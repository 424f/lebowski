namespace Lebowski.TextModel.Operations
{
    /// <summary>
    /// A visitor on <see cref="TextOperation">TextOperation</see> objects.
    /// </summary>
    /// <remarks>
    /// The generic class ITextOperationVisitor&lt;T&gt; describes a visitor
    /// 
    /// </remarks>
    public interface ITextOperationVisitor<ReturnType>
    {
        /// <summary>
        /// Visits a <see cref="TextOperation">TextOperation</see>.
        /// </summary>
        /// <param name="textOperation">The operation to visit.</param>
        /// <returns>Result of the visitation.</returns>
        ReturnType VisitInsertOperation(InsertOperation textOperation);
        
        /// <summary>
        /// Visits a <see cref="DeleteOperation">DeleteOperation</see>.
        /// </summary>
        /// <param name="deleteOperation">The operation to visit.</param>
        /// <returns>Result of the visitation.</returns>        
        ReturnType VisitDeleteOperation(DeleteOperation deleteOperation);
    }
}