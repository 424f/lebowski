namespace TwinEditor
{
    using System;
    using System.IO;
    using IronPython;
    using IronPython.Hosting;
    using IronPython.Runtime;
    using Microsoft.Scripting;
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// <param>A IronPython Interpreter</param>
    /// <param></param>
    /// </summary>
    public class PythonInterpreter
    {
        /// <value>
        /// The Engine that executes the IronPython Code.
        /// </value>
        private ScriptEngine engine = null;
        /// <value>
        /// The runtime of the execution engine.
        /// </value>
        private ScriptRuntime runtime = null;
        /// <value>
        /// The Scope that holds all variables, functions and class definitions.
        /// </value>
        private ScriptScope scope = null;

        /// <summary>
        /// Initializes a new instance of PythonInterpreter.
        /// </summary>
        public PythonInterpreter()
        {
            this.InitializePythonEngine();
        }

        /// <summary>
        /// Initializes the Scriptengine, its runtime and the Scriptscope.
        /// </summary>
        private void InitializePythonEngine()
        {
            engine = Python.CreateEngine();


            scope = engine.CreateScope();

            runtime = engine.Runtime;
        }

        /// <summary>
        /// Executes the code.
        /// </summary>
        /// <param name="code">Source code to be executed.</param>
        /// <returns>Object that encapsulates the compiles code.</returns>
        public object ExecuteCode(string code)
        {
            return ExecuteCode(code, new PythonStdoutWriter());
        }

        /// <summary>
        /// Executes the code and writes the results in writer.
        /// </summary>
        /// <param name="code">Source code to be executed.</param>
        /// <param name="writer">The result is written here.</param>
        /// <returns>Object that encapsulates the compiles code.</returns>
        public object ExecuteCode(string code, PythonWriter writer)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            Object o = null;
            try
            {
                CompiledCode compiled = source.Compile();
                object sys = runtime.GetSysModule();
                engine.Operations.SetMember(sys, "stdout", writer);
                engine.Operations.SetMember(sys, "stderr", writer);

                o = compiled.Execute(scope);

                // TODO: allow canceling operation
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                writer.write(error);
            }
            return o;
        }
    }

    /// <summary>
    /// Specific EventArgs for WriteEvent
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

    /// <summary>
    ///  An abstract writer that can fire a Write event.
    /// </summary>
    public abstract class PythonWriter
    {
        /// <summary>
        /// Write the text.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        public abstract void write(string text);

        /// <summary>
        /// The Write event that can be triggered on each write.
        /// </summary>
        public event EventHandler<WriteEventArgs> Write;

        /// <summary>
        /// Fires the Write event.
        /// </summary>
        /// <param name="e">A <see cref="WriteEventArgs"></see>that contains the event data.</param>
        protected virtual void OnWrite(WriteEventArgs e)
        {
            if (Write != null)
            {
                Write(this, e);
            }
        }

        public int softspace { get; set; }

    }

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

    /// <summary>
    /// A <see cref="PythonWriter"></see>that write to a StringWriter and fires a Write event upen every write.
    /// </summary>
    public class PythonStringWriter : PythonWriter
    {
        /// <summary>
        /// The StringWriter to be written to
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