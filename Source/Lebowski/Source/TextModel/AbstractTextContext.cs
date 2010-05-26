

namespace Lebowski.TextModel
{
    using System;
    /// <summary>
    /// Provides functionality that most implementing classes of ITextContext
    /// share.
    /// </summary>
    public abstract class AbstractTextContext : ITextContext
    {
        /// <inheritdoc/>
        public virtual int CaretPosition { get; set; }        
        /// <inheritdoc/>
        public abstract string Data { get; set;    }        
        /// <inheritdoc/>
        public abstract bool HasSelection { get; }        
        /// <inheritdoc/>
        public abstract int SelectionStart { get; protected set;}        
        /// <inheritdoc/>
        public abstract int SelectionEnd { get; protected set; }        
        
        /// <inheritdoc/>
        public string SelectedText
        {
            get
            {
                return Data.Substring(SelectionStart, SelectionEnd-SelectionStart);
            }
        }        
        
        /// <inheritdoc/>
        public event EventHandler<ChangeEventArgs> Changed;
        /// <inheritdoc/>
        public event EventHandler<DeleteEventArgs> Deleted;
        /// <inheritdoc/>
        public event EventHandler<InsertEventArgs> Inserted;                

        /// <inheritdoc/>
        public abstract void Delete(object issuer, Lebowski.TextModel.Operations.DeleteOperation operation);
        /// <inheritdoc/>
        public abstract void Invoke(Action action);
        /// <inheritdoc/>
        public abstract void Insert(object issuer, Lebowski.TextModel.Operations.InsertOperation operation);        
        
        /// <summary>
        /// Raises the Inserted event.
        /// </summary>
        /// <param name="e">A InsertEventArgs that contains the event data.</param>
        protected virtual void OnInserted(InsertEventArgs e)
        {
            if (Inserted != null) {
                Inserted(this, e);
            }
        }

        /// <summary>
        /// Raises the Deleted event.
        /// </summary>
        /// <param name="e">A DeleteEventArgs that contains the event data.</param>        
        protected virtual void OnDeleted(DeleteEventArgs e)
        {
            if (Deleted != null) {
                Deleted(this, e);
            }
        }

        /// <summary>
        /// Raises the Changed event.
        /// </summary>
        /// <param name="e">A ChangeEventArgs that contains the event data.</param>        
        protected virtual void OnChanged(ChangeEventArgs e)
        {
            if (Changed != null) {
                Changed(this, e);
            }
        }

        /// <inheritdoc/>
        public abstract void Refresh();
        /// <inheritdoc/>
        public abstract void SetSelection(int start, int last);
        /// <inheritdoc/>
        public abstract void SetRemoteSelection(object siteIdentifier, int start, int end);
    }
}