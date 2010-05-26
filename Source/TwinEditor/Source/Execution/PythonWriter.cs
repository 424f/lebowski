
using System;

namespace TwinEditor.Execution
{
    /// <summary>
    /// A writer that can be passed into an IronPython execution to capture
    /// data written to a stream.
    /// </summary>
    public abstract class PythonWriter
    {
        /// <summary>
        /// Write the text.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        public abstract void write(string text);

        /// <summary>
        /// Fires the Write event.
        /// </summary>
        /// <param name="e">A <see cref="WriteEventArgs">WriteEventArgs</see>that contains the event data.</param>
        protected virtual void OnWrite(WriteEventArgs e)
        {
            if (Write != null)
            {
                Write(this, e);
            }
        }        
        
        /// <summary>
        /// The Write event that can be triggered on each write.
        /// </summary>
        public event EventHandler<WriteEventArgs> Write;

        /// <summary>
        /// Used for example in statements like &quot;print "Foo",&quot;
        /// </summary>
        public int softspace { get; set; }

    }
}
