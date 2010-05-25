namespace Lebowski.TextModel
{
    using System;
    using Lebowski.TextModel.Operations;

    public class StringTextContext : AbstractTextContext
    {
        public override string Data { get; set; }
        public override int SelectionStart { get; protected set; }
        public override int SelectionEnd { get; protected set; }

        public StringTextContext()
        {
            Data = "";
        }

        public override void Insert(object issuer, InsertOperation operation)
        {
            Data = Data.Substring(0, operation.Position) + operation.Text + Data.Substring(operation.Position);
            OnInserted(new InsertEventArgs(issuer, operation));
            OnChanged(new ChangeEventArgs(issuer));
        }

        public override void Delete(object issuer, DeleteOperation operation)
        {
            Data = Data.Substring(0, operation.Position) + Data.Substring(operation.Position+1);
            OnDeleted(new DeleteEventArgs(issuer, operation));
            OnChanged(new ChangeEventArgs(issuer));
        }

        public override void SetSelection(int start, int last)
        {
            SelectionStart = start;
            SelectionEnd = last;
        }

        public override bool HasSelection
        {
            get { return true; }
        }

        public override void Invoke(Action d)
        {
            d();
        }

        public override void Refresh()
        {

        }

        public override void SetRemoteSelection(object siteIdentifier, int start, int end)
        {

        }
    }
}