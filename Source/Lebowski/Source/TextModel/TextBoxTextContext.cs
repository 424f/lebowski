namespace Lebowski.TextModel
{
    using System;
    using System.Windows.Forms;
    using Lebowski.TextModel.Operations;

    /// <summary>
    /// TextBoxTextContext is a text context that is linked with a standard
    /// WinForms <see cref="System.Windows.Forms">TextBox</see>. It does not
    /// currently support display of remote selections.
    /// </summary>
    public class TextBoxTextContext : AbstractTextContext
    {
        /// <summary>
        /// The GUI element this context is linked with.
        /// </summary>
        private TextBox TextBox;

        /// <summary>
        /// Initializes a new instance of the TextBoxTextContext.
        /// </summary>
        /// <param name="textBox"></param>
        public TextBoxTextContext(TextBox textBox)
        {
            TextBox = textBox;
            TextBox.TextChanged += TextBoxChanged;

            // TODO: implement insert / delete
        }        

        /// <inheritdoc/>        
        public override string Data
        {
            get { return TextBox.Text; }
            set
            {
                TextBox.TextChanged -= TextBoxChanged;
                TextBox.Text = value;
                TextBox.TextChanged += TextBoxChanged;
            }
        }        
        
        /// <inheritdoc/>
        public override bool HasSelection
        {
            get { return true; }
        }        
    
        /// <inheritdoc/>        
        public override int SelectionStart
        {
            get { return TextBox.SelectionStart; }
            protected set { TextBox.SelectionStart = value; }
        }

        /// <inheritdoc/>        
        public override int SelectionEnd
        {
            get { return TextBox.SelectionStart + TextBox.SelectionLength; }
            protected set { TextBox.SelectionLength = value - TextBox.SelectionStart; }
        }

        /// <inheritdoc/>        
        public override void Delete(object issuer, DeleteOperation operation)
        {
            TextBox.TextChanged -= TextBoxChanged;
            TextBox.Text.Remove(operation.Position, 1);
            TextBox.TextChanged += TextBoxChanged;
        }        
        
        /// <inheritdoc/>
        public override void Invoke(Action d)
        {
            TextBox.Invoke(d);
        }        
        
        /// <inheritdoc/>        
        public override void Insert(object issuer, InsertOperation operation)
        {
            TextBox.TextChanged -= TextBoxChanged;
            TextBox.Text.Insert(operation.Position, operation.Text.ToString());
            TextBox.TextChanged += TextBoxChanged;
        }        

        /// <inheritdoc/>
        public override void Refresh()
        {
            TextBox.Refresh();
        }        

        /// <inheritdoc/>
        public override void SetRemoteSelection(object siteIdentifier, int start, int end)
        {
            /* Pure WinForms TextBox doesn't give us an easy way to display
            additional selections */
        }        
        
        /// <inheritdoc/>        
        public override void SetSelection(int start, int last)
        {
            TextBox.SelectionStart = start;
            TextBox.SelectionLength = last - start;
        }
        
        /// <summary>
        /// Handler for the Changed event issued by the TextBox associated
        /// with this context.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The data for this Changed event.</param>
        private void TextBoxChanged(object sender, EventArgs e)
        {
            OnChanged(new ChangeEventArgs(this));
        }
    }
}