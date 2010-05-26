namespace TwinEditor.Execution
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
        /// Initializes the ScriptEngine, its runtime and the scope.
        /// </summary>
        private void InitializePythonEngine()
        {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();
            runtime = engine.Runtime;
        }

        /// <summary>
        /// Executes a source file.
        /// </summary>
        /// <param name="code">Source code to be executed.</param>
        /// <returns>Object that encapsulates the compiles code.</returns>
        public object ExecuteCode(string code)
        {
            return ExecuteCode(code, new PythonStdoutWriter());
        }

        /// <summary>
        /// Executes the code and redirects the results into a writer object.
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
}