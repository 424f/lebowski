/*
 * Created by SharpDevelop.
 * User: laiw
 * Date: 30.04.2010
 * Time: 15:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting; 

namespace TwinEditor
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class PythonInterpreter
	{
		
		private ScriptEngine pyEngine = null;
		private ScriptRuntime pyRuntime = null;
		private ScriptScope pyScope = null;
		
		public PythonInterpreter()
		{
			this.initializePythonEngine();
		}
		
		private void initializePythonEngine() {
            pyEngine = Python.CreateEngine();
            pyScope = pyEngine.CreateScope();
		}
		
		public void CompileSourceAndExecute(String code)
		{
	    ScriptSource source = pyEngine.CreateScriptSourceFromString
					(code, SourceCodeKind.Statements);
	    CompiledCode compiled = source.Compile();
	    // Executes in the scope of Python
	    compiled.Execute(pyScope);
		}
		
	}
}