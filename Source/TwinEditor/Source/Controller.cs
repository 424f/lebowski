using System;
using System.Collections.Generic;

namespace TwinEditor
{
	public class Controller
	{	
		internal PythonInterpreter py;
		private int fileNumber;
		
		public Controller()
		{	
			this.py = new PythonInterpreter();
			this.fileNumber = 0;
		}
		
		public int GetNextFileNumber() {
			return ++fileNumber;
		}
		
		public void ExecutePython(string statement) {
			py.ExecuteCode(statement);
		}
		
		
		
	}
}
