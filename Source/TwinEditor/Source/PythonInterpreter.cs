using System;
using System.IO;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace TwinEditor
{
	public class PythonInterpreter
	{
		private ScriptEngine engine = null;
		private ScriptRuntime runtime = null;
		private ScriptScope scope = null;
		
		public PythonInterpreter()
		{
			this.InitializePythonEngine();
		}
		
		private void InitializePythonEngine()
		{
            engine = Python.CreateEngine();
            scope = engine.CreateScope();
            runtime = engine.Runtime;
		}
		
		public object ExecuteCode(string code)
		{
			return ExecuteCode(code, new PythonStdoutWriter());
		}
			
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
	
	public interface PythonWriter
	{
		void write(string text);
	}
	
	public class PythonStdoutWriter : PythonWriter
	{
		public void write(string text)
		{
			Console.Write(text);
		}
	}
	
	public class PythonStringWriter : PythonWriter
	{
		private StringWriter writer = new StringWriter();
		
		public void write(string text)
		{
			writer.Write(text);
		}
		
		public string GetContent()
		{
			return writer.GetStringBuilder().ToString();
		}
	}
}