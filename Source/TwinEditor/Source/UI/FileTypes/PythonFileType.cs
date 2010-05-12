using System;
using System.IO;
using System.Drawing;

namespace TwinEditor.UI.FileTypes
{
	public class PythonFileType : IFileType
	{
		private PythonInterpreter interpreter;
		
		public PythonFileType()
		{
			interpreter = new PythonInterpreter();
		}
		
		public string Name
		{
			get { return "Python"; }
		}
		
		public string FileNamePattern
		{
			get { return "*.py"; }
		}
		
		public string FileExtension
		{
			get { return ".py"; }
		}
	
		public bool FileNameMatches(string fileName)
		{
			return fileName.EndsWith(FileExtension);
		}
		
		public Image Icon
		{
			get { return Image.FromFile("../../../../Resources/Icons/python.png"); }
		}
		
		public bool CanCompile
		{
			get { return false; }
		}
		
		public void Compile(string content, TextWriter stdout)
		{
			
		}
		
		public bool CanExecute
		{
			get { return true; }
		}		
		
		public void Execute(string content, TextWriter stdout)
		{
			var writer = new PythonStringWriter();
			interpreter.ExecuteCode(content, writer);
			string output = writer.GetContent().ToString();
			output = output.Replace("\n", Environment.NewLine);
			stdout.Write(output);
		}		
	}
}
