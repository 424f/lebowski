
using System;

namespace TwinEditor.Execution
{
    /// <summary>
    /// A <see cref="PythonWriter"></see>that writes to System.Console and fires a Write event upon every write.
    /// </summary>
    public class PythonStdoutWriter : PythonWriter
    {
        /// <summary>
        /// Writes the text to System.Console
        /// </summary>
        /// <param name="text">Text to be written.</param>
        public override void write(string text)
        {
            Console.Write(text);
            OnWrite(new WriteEventArgs(text));
        }
    }
}
