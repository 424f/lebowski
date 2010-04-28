using System;

namespace Lebowski.UI.FileTypes
{
	public class PythonFileType : IFileType
	{
		public string FileNamePattern {
			get { return "*.py"; }
		}
		
	}
}
