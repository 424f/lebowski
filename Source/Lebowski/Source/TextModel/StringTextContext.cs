namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// A text context that does not have any GUI representation, but just
    /// applies operations to a string instead. This is useful for performing
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
        /// Initializes a new instance of the StringTextContext where 
        /// the document initially is not necessarily empty.
        /// </summary>        
        public StringTextContext(string data)
        {
            Data = data;
        }
        
        /// <summary>
        /// The start index of the remote selection.
        /// </summary>
        public int RemoteSelectionStart { get; private set; }
        
        /// <summary>
        /// The end index of the remote selection.
        /// </summary>
        public int RemoteSelectionEnd { get; private set; }
        
        /// <summary>
        /// Stores the text associated with this context. Note that for 
        /// the StringTextContext, we don't have a GUI element corresponding
        /// to this data.
        /// </summary>
        public override string Data
        {
            get { return data; }
            set
            {
                data = value;
            }
        }
        private string data;
        
        /// <summary>
        /// Behaves the same as this.Data=data, but also issues a event
        /// that the text context has changed, as if a user had changed it.
        /// </summary>
        /// <param name="data">the new context data</param>
        public void SetDataAsUser(string data)
        {
            Data = data;
            OnChanged(new ChangeEventArgs(null));            
        }
        
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
            RemoteSelectionStart = start;
            RemoteSelectionEnd = end;
        }        
        
        /// <inheritdoc/>
        public override void SetSelection(int start, int last)
        {
            if (start > last)
                throw new ArgumentException();
            if (last > (Data.Length))
                throw new ArgumentOutOfRangeException();

            SelectionStart = start;
            SelectionEnd = last;
        }        
        
        /// <summary>
        /// Behaves like SetSelection but also 
        /// </summary>
        /// <param name="start">The first index part of the selection.</param>
        /// <param name="last">The index of the character after the last selected character.</param>
        public void SetSelectionAsUser(int start, int last)
        {
            SetSelection(start, last);
            OnChanged(new ChangeEventArgs(null));
        }
    }
}