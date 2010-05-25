namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;
    
    public interface ITextContext
    {
        void Refresh();
        
        string Data { get; set; }
        
        /// <summary>
        /// Returns the index of the first character that is part of the selection
        /// </summary>
        int SelectionStart { get; }
        
        /// <summary>
        /// Returns the index of the first character that is not part of the selection
        /// </summary>
        int SelectionEnd { get; }
        
        /// <summary>
        /// Returns true when the there currently is a selection
        /// </summary>
        bool HasSelection { get; }
        
        int CaretPosition { get; set; }
        string SelectedText { get; }
        
        void Insert(object issuer, InsertOperation operation);
        void Delete(object issuer, DeleteOperation operation);
        void SetSelection(int start, int last);
        
        /// <summary>
        /// Sets the remote selection of the user identified by <value>siteIdentifier</value>.
        /// Some text context implementations might support displaying those 
        /// remote selections to achieve better interaction awareness.
        /// </summary>
        /// <param name="siteIdentifier">A identifier that uniquely describes the remote site in the current session</param>
        /// <param name="start">Start of the selection</param>
        /// <param name="end">End of the selection</param>
        void SetRemoteSelection(object siteIdentifier, int start, int end);
        
        /// <summary>
        /// Invokes the given action in a thread that has read-write access
        /// to this context. If you are not operating on e.g. a UI control,
        /// you can just implement this method by calling the delegate directly.
        /// </summary>
        void Invoke(Action action);
        
        event EventHandler<InsertEventArgs> Inserted;
        event EventHandler<DeleteEventArgs> Deleted;
        event EventHandler<ChangeEventArgs> Changed;
    }
    
    public class InsertEventArgs : EventArgs
    {
        public InsertOperation Operation { get; protected set; }
        public object Issuer { get; protected set; }
        
        public InsertEventArgs(object issuer, InsertOperation operation)
        {
            Operation = operation;
            Issuer = issuer;
        }
    }
    
    public class DeleteEventArgs : EventArgs
    {
        public DeleteOperation Operation { get; protected set; }
        public object Issuer { get; protected set; }
        
        public DeleteEventArgs(object issuer, DeleteOperation operation)
        {
            Operation = operation;
            Issuer = issuer;
        }
    }    
    
    public class ChangeEventArgs : EventArgs
    {
        public object Issuer { get; protected set; }
        
        public ChangeEventArgs(object issuer)
        {
            Issuer = issuer;
        }
    }
}
