
namespace Lebowski.Synchronization.DifferentialSynchronization
{
    using System;
    /// <summary>
    /// A message that is transmitted to get two <see cref="ITextContext">ITextContext</see>
    /// </summary>
    [Serializable]
    internal sealed class DiffMessage
    {
        /// <summary>
        /// The delta for the diff, based on the common shadow
        /// </summary>
        public string Delta { get; private set; }
        
        /// <summary>
        /// The user's selection when this message was sent
        /// </summary>
        public int SelectionStart { get; private set; }
        public int SelectionEnd { get; private set; }
        
        public DiffMessage(string delta)
        {
            Delta = delta;
        }

        public DiffMessage(string delta, int selectionStart, int selectionEnd) : this(delta)
        {
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
        }        
        
        public override string ToString()
        {
            return String.Format("DiffMessage({0})", Delta);
        }
    }
}
