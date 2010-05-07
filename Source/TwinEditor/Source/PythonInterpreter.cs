using System;
using System.IO;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;  
using NUnit.Framework;

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
		    CompiledCode compiled = source.Compile();
		    object sys = runtime.GetSysModule();
		    engine.Operations.SetMember(sys, "stdout", writer);
		    engine.Operations.SetMember(sys, "stderr", writer);
		    Object o = null;
		    try {
		    	o = compiled.Execute(scope);
		    } catch (Exception e) {
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
	
	[TestFixture]
	public class PythonInterpreterTest
	{
		
		[Test]
		public void testExecution()
		{
			var interpreter = new PythonInterpreter();
			var writer = new PythonStringWriter();
			object val = interpreter.ExecuteCode("print 'test'", writer);
			Assert.AreEqual("test\n", writer.GetContent());
		}		
		
		[Test]
		public void testExecution2()
		{
			var interpreter = new PythonInterpreter();
			var writer = new PythonStringWriter();
			string code = @"
def fact(n):
  return (1 if n <= 1 else n*fact(n-1))
for i in xrange(5):
  print fact(i)";
			object val = interpreter.ExecuteCode(code, writer);
			Assert.AreEqual("1\n1\n2\n6\n24\n", writer.GetContent());
		}				
	}
	
	
}