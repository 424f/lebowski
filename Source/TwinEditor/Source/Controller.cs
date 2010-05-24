using System;
using System.Collections.Generic;

namespace TwinEditor
{
	public class Controller
	{	
		private int fileNumber;
		
		public Controller()
		{	
			this.fileNumber = 0;
		}
		
		public int GetNextFileNumber() {
			return ++fileNumber;
		}
	}
}
