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
	
	public sealed class WriteEventArgs : EventArgs
	{
	    public string Text { get; private set; }
	    
	    public WriteEventArgs(string text)
	    {
	        Text = text;
	    }
	}
	
	public abstract class PythonWriter
	{
		public abstract void write(string text);
	
		public event EventHandler<WriteEventArgs> Write;
		
        protected virtual void OnWrite(WriteEventArgs e)
        {
            if (Write != null)
            {
                Write(this, e);
            }
        }
        
        public int softspace { get; set; }
		
	}
	
	public class PythonStdoutWriter : PythonWriter
	{
		public override void write(string text)
		{
			Console.Write(text);
			OnWrite(new WriteEventArgs(text));
		}
	}
	
	public class PythonStringWriter : PythonWriter
	{
		private StringWriter writer = new StringWriter();
		
		public override void write(string text)
		{
			writer.Write(text);
			OnWrite(new WriteEventArgs(text));
		}
		
		public string GetContent()
		{
			return writer.GetStringBuilder().ToString();
		}
	}
}