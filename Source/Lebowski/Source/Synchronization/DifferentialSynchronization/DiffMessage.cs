using System;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
    [Serializable]
    internal sealed class DiffMessage
    {
        /// <summary>
        /// The diff based on the common shadow
        /// </summary>
        public string Diff { get; private set; }
        
        /// <summary>
        /// The user's selection when this message was sent
        /// </summary>
        public int SelectionStart { get; private set; }
        public int SelectionEnd { get; private set; }
        
        public DiffMessage(string diff)
        {
            Diff = diff;
        }

        public DiffMessage(string diff, int selectionStart, int selectionEnd)
        {
            Diff = diff;
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
        }        
        
        public override string ToString()
        {
            return String.Format("DiffMessage({0})", Diff);
        }
    }
}
