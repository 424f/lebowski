
namespace Lebowski.Synchronization.DifferentialSynchronization
{
    using System;
    /// <summary>
    /// A message that is transmitted to get two <see cref="Lebowski.TextModel.ITextContext">ITextContext</see> 
    /// into the same state.
    /// </summary>
    [Serializable]
    internal sealed class DiffMessage
    {
        /// <summary>
        /// Initializes a new instance of the DiffMessage class.
        /// </summary>
        /// <param name="delta">The delta of the diff created at the initiating site.</param>
        public DiffMessage(string delta)
        {
            Delta = delta;
        }

        /// <summary>
        /// Initializes a new instance of the DiffMessage class.
        /// </summary>
        /// <param name="delta"><see cref="Delta">Delta</see></param>
        /// <param name="selectionStart"><see cref="SelectionStart">SelectionStart</see></param>
        /// <param name="selectionEnd"><see cref="SelectionEnd">SelectionEnd</see></param>
        public DiffMessage(string delta, int selectionStart, int selectionEnd) : this(delta)
        {
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
        }        
        
        /// <summary>
        /// The delta for the diff, based on the common shadow
        /// </summary>
        public string Delta { get; private set; }

        /// <summary>
        /// The user's selection's start when this message was sent
        /// </summary>
        public int SelectionStart { get; private set; }
        
        /// <summary>
        /// The user's selection's end when this message was sent
        /// </summary>        
        public int SelectionEnd { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.Format("DiffMessage({0})", Delta);
        }
    }
}