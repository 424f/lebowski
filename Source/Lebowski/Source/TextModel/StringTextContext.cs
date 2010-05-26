namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// A text context that does not have any GUI representation, but just
    /// applies operations to a string instead. This is usefull for performing
    /// text operations on a shared context.
    /// </summary>
    public class StringTextContext : AbstractTextContext
    {
        /// <summary>
        /// Initializes a new instance of the StringTextContext class.
        /// </summary>
        public StringTextContext()
        {
            Data = "";
        }        
        
        /// <summary>
        /// Stores the text associated with this context. Note that for 
        /// the StringTextContext, we don't have a GUI element corresponding
        /// to this data.
        /// </summary>
        public override string Data { get; set; }
        
        /// <inheritdoc/>
        public override bool HasSelection
        {
            get { return true; }
        }        
        
        /// <inheritdoc/>
        public override int SelectionStart { get; protected set; }
        
        /// <inheritdoc/>
        public override int SelectionEnd { get; protected set; }

        /// <inheritdoc/>
        public override void Delete(object issuer, DeleteOperation operation)
        {
            Data = Data.Substring(0, operation.Position) + Data.Substring(operation.Position+1);
            OnDeleted(new DeleteEventArgs(issuer, operation));
            OnChanged(new ChangeEventArgs(issuer));
        }

        /// <inheritdoc/>
        public override void Insert(object issuer, InsertOperation operation)
        {
            Data = Data.Substring(0, operation.Position) + operation.Text + Data.Substring(operation.Position);
            OnInserted(new InsertEventArgs(issuer, operation));
            OnChanged(new ChangeEventArgs(issuer));
        }

        /// <inheritdoc/>
        public override void Invoke(Action d)
        {
            d();
        }        
        
        /// <inheritdoc/>
        public override void Refresh()
        {

        }        

        /// <inheritdoc/>
        public override void SetRemoteSelection(object siteIdentifier, int start, int end)
        {

        }        
        
        /// <inheritdoc/>
        public override void SetSelection(int start, int last)
        {
            SelectionStart = start;
            SelectionEnd = last;
        }        
    }
}