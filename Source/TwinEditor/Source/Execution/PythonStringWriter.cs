using System;

namespace TwinEditor.Execution
{
    using System.IO;
    
    /// <summary>
    /// A <see cref="PythonWriter"></see>that captures writes and allows
    /// access to all written data.
    /// </summary>
    public class PythonStringWriter : PythonWriter
    {
        /// <summary>
        /// The StringWriter to be written to.
        /// </summary>
        private StringWriter writer = new StringWriter();

        /// <summary>
        /// Writes the text to <see cref="writer"></see>
        /// </summary>
        /// <param name="text">Text to be written.</param>
        public override void write(string text)
        {
            writer.Write(text);
            OnWrite(new WriteEventArgs(text));
        }

        /// <summary>
        /// Gets the content of the StringWriter
        /// </summary>
        /// <returns>String of the StringWriter</returns>
        public string GetContent()
        {
            return writer.GetStringBuilder().ToString();
        }
    }
}
