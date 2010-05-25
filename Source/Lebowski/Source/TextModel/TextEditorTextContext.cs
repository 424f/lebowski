// TODO: extract common functionality with TextBoxTextContext into common abstract superclass

namespace Lebowski.TextModel
{
    using System;
    using ICSharpCode.TextEditor;    
    using ICSharpCode.TextEditor.Document;
    using Lebowski.TextModel.Operations;    
    
    public class TextEditorTextContext : AbstractTextContext
    {
        // The marker used to highlight selection of the remote party
        private TextMarker remoteMarker;
        
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
        
        public override void Insert(object issuer, InsertOperation operation)
        {
            UnsubscribeFromTextBox();
            TextBox.Text.Insert(operation.Position, operation.Text.ToString());
            SubscribeToTextBox();
        }
        
        public override void Delete(object issuer, DeleteOperation operation)
        {
            UnsubscribeFromTextBox();
            TextBox.Text.Remove(operation.Position, 1);
            SubscribeToTextBox();
        }
        
        private void SubscribeToTextBox()
        {
            TextBox.TextChanged += TextBoxChanged;
            TextBox.ActiveTextAreaControl.SelectionManager.SelectionChanged += TextBoxChanged;
        }        
        
        private void UnsubscribeFromTextBox()
        {
            TextBox.TextChanged -= TextBoxChanged;
            TextBox.ActiveTextAreaControl.SelectionManager.SelectionChanged -= TextBoxChanged;
        }
        
        TextEditorControl TextBox;
        
        public TextEditorTextContext(TextEditorControl textBox)
        {
            TextBox = textBox;
            SubscribeToTextBox();
            
            remoteMarker = new TextMarker(0, 0, TextMarkerType.SolidBlock, System.Drawing.Color.Yellow);
            
            // TODO: implement insert / delete
        }
        
        public void TextBoxChanged(object sender, EventArgs e)
        {
            OnChanged(new ChangeEventArgs(this));
        }
        
        public override void SetSelection(int start, int last)
        {    
            UnsubscribeFromTextBox();
            TextLocation startLocation = TextBox.Document.OffsetToPosition(start);
            TextLocation lastLocation = TextBox.Document.OffsetToPosition(last);
            TextBox.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, lastLocation);
            SubscribeToTextBox();
        }            
        
        public override bool HasSelection
        {
            get
            {
                return TextBox.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0 &&
                         SelectionStart != SelectionEnd;
            }
        }        
        
        public override void Invoke(Action d) 
        {
            TextBox.Invoke(d);
        }
        
        public override void Refresh()
        {
            TextBox.Refresh();
        }
        
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
    }
}
