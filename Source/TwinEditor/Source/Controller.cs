/*
 * Created by SharpDevelop.
 * User: laiw
 * Date: 30.04.2010
 * Time: 10:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TwinEditor
{
	/// <summary>
	/// Description of Controller.
	/// </summary>
	public class Controller
	{	
		internal PythonInterpreter py;
		private int fileNumber;
		
		public Controller()
		{	
			this.py = new PythonInterpreter();
			this.fileNumber = 0;
		}
		
		public int getNextFileNumber() {
			return ++fileNumber;
		}
		
		public void executePython(string statement) {
			py.CompileSourceAndExecute(statement);
		}
		
		
		
	}
}
