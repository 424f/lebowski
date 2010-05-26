namespace Lebowski.TextModel
{
    using System;
    using ICSharpCode.TextEditor;
    using ICSharpCode.TextEditor.Document;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// TextBoxTextContext is a text context that is linked with a standard
    /// ICSharpCode.TextEditor that supports syntax hightlighting, display
    /// of remote selections
    /// currently support display of remote selections.
    /// </summary>    
    public class TextEditorTextContext : AbstractTextContext
    {
        // The marker used to highlight selection of the remote party
        private TextMarker remoteMarker;

        // The UI element this context is associated with.
        TextEditorControl TextBox;                
        
        /// <summary>
        /// Initializes a new instance of the TextEditorTextContent class.
        /// This TextEditorTextContent instance will be linked with a
        /// TextEditorControl.
        /// </summary>
        /// <param name="textBox">The TextEditorControl to link with.</param>
        public TextEditorTextContext(TextEditorControl textBox)
        {
            TextBox = textBox;
            SubscribeToTextBox();

            remoteMarker = new TextMarker(0, 0, TextMarkerType.SolidBlock, System.Drawing.Color.Yellow);

            // TODO: implement insert / delete
        }        

        /// <inheritdoc/>
        public override int CaretPosition
        {
            get
            {
                return TextBox.ActiveTextAreaControl.Caret.Offset;
            }

            set
            {
                TextBox.ActiveTextAreaControl.Caret.Position = TextBox.Document.OffsetToPosition(value);
            }
        }

        /// <inheritdoc/>
        public override string Data
        {
            get { return TextBox.Text; }
            set
            {
                UnsubscribeFromTextBox();
                TextBox.Text = value;
                SubscribeToTextBox();
            }
        }
        
        /// <inheritdoc/>
        public override bool HasSelection
        {
            get
            {
                return TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0 &&
                         SelectionStart != SelectionEnd;
            }
        }        
        
        /// <inheritdoc/>
        public override int SelectionStart
        {
            get
            {
                if (!TextBox.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                    return 0;
                return TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
            }
            protected set { throw new NotImplementedException(); }
        }

        /// <inheritdoc/>
        public override int SelectionEnd
        {
            get
            {
                if (!TextBox.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                    return 0;
                return SelectionStart + TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Length;
            }
            protected set { throw new NotImplementedException(); }
        }        

        /// <inheritdoc/>
        public override void Delete(object issuer, DeleteOperation operation)
        {
            UnsubscribeFromTextBox();
            TextBox.Text.Remove(operation.Position, 1);
            SubscribeToTextBox();
        }        
        
        /// <inheritdoc/>
        public override void Insert(object issuer, InsertOperation operation)
        {
            UnsubscribeFromTextBox();
            TextBox.Text.Insert(operation.Position, operation.Text.ToString());
            SubscribeToTextBox();
        }

        /// <inheritdoc/>
        public override void Invoke(Action d)
        {
            TextBox.Invoke(d);
        }        

        /// <inheritdoc/>        
        public override void Refresh()
        {
            TextBox.Refresh();
        }        

        /// <inheritdoc/>
        public override void SetRemoteSelection(object siteIdentifier, int start, int end)
        {
            TextBox.Document.MarkerStrategy.RemoveMarker(remoteMarker);
            remoteMarker.Offset = start;
            remoteMarker.Length = end - start;
            if(start != end)
            {
                TextBox.Document.MarkerStrategy.AddMarker(remoteMarker);
            }
        }                
        
        /// <inheritdoc/>
        public override void SetSelection(int start, int last)
        {
            UnsubscribeFromTextBox();
            TextLocation startLocation = TextBox.Document.OffsetToPosition(start);
            TextLocation lastLocation = TextBox.Document.OffsetToPosition(last);
            TextBox.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, lastLocation);
            SubscribeToTextBox();
        }        
        
        /// <summary>
        /// Ensures that this instance is subscribed to all TextBox events
        /// needed to itself issue the right events.
        /// </summary>
        private void SubscribeToTextBox()
        {
            TextBox.TextChanged += TextBoxChanged;
            TextBox.ActiveTextAreaControl.SelectionManager.SelectionChanged += TextBoxChanged;
        }

        /// <summary>
        /// Handles changes in the associated UI element.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The data associated with the event.</param>
        private void TextBoxChanged(object sender, EventArgs e)
        {
            OnChanged(new ChangeEventArgs(this));
        }
                
        /// <summary>
        /// Ensures that changes to the UI element do not fire any events
        /// within this instance. This is useful as we usually do not want
        /// changes applied by this instance to be again reported back to us.
        /// </summary>        
        private void UnsubscribeFromTextBox()
        {
            TextBox.TextChanged -= TextBoxChanged;
            TextBox.ActiveTextAreaControl.SelectionManager.SelectionChanged -= TextBoxChanged;
        }
    }
}