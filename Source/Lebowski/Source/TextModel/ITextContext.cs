namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// A text context, that contains the state of a collaboratively shared
    /// document.
    /// </summary>
    public interface ITextContext
    {
        /// <summary>
        /// Gets or sets the position of the local caret within the document.
        /// </summary>
        int CaretPosition { get; set; }
        
        /// <summary>
        /// The data (text) that makes up the document.
        /// </summary>
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

        /// <summary>
        /// Gets the text currently selected.
        /// </summary>
        string SelectedText { get; }

        /// <summary>
        /// Performs a deletion operation on the context.
        /// </summary>
        /// <param name="issuer">The object that calls Delete.</param>
        /// <param name="operation">The DeleteOperation that should be executed.</param>
        void Delete(object issuer, DeleteOperation operation);
        
        /// <summary>
        /// Performs a insertion operation on the context.
        /// </summary>
        /// <param name="issuer">The object that calls Insert.</param>
        /// <param name="operation">The InsertOperation that should be executed.</param>
        void Insert(object issuer, InsertOperation operation); 
        
        /// <summary>
        /// Refreshes the context, making sure that changes are correctly 
        /// reflected in any UI associated with it.
        /// </summary>
        void Refresh();
        
        /// <summary>
        /// Sets the local selection.
        /// </summary>
        /// <param name="start">The index of the first character that should be selected.</param>
        /// <param name="last">The index after the last character that should be selected.</param>
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

        /// <summary>
        /// Occurs when the context has been changed.
        /// </summary>
        event EventHandler<ChangeEventArgs> Changed;
        
        /// <summary>
        /// Occurs when an DeleteOperation has been performed.
        /// </summary>
        event EventHandler<DeleteEventArgs> Deleted;        
        
        /// <summary>
        /// Occurs when an InsertOperation has been performed.
        /// </summary>
        event EventHandler<InsertEventArgs> Inserted;
    }
}