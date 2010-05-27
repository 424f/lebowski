namespace TwinEditor.Execution
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="PythonWriter.Write" /> event.
    /// </summary>
    public sealed class WriteEventArgs : EventArgs
    {
        /// <value>
        /// Text to be passed
        /// </value>
        public string Text { get; private set; }

        /// <summary>
        /// Initializes a new instance of WriteEventArgs
        /// </summary>
        /// <param name="text"></param>
        public WriteEventArgs(string text)
        {
            Text = text;
        }
    }
}
