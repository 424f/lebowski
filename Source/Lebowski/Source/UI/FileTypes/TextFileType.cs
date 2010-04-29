using System;
using System.IO;
using System.Drawing;

namespace Lebowski.UI.FileTypes
{
	public class TextFileType : IFileType
	{
		public string Name
		{
			get { return "Text file"; }
		}
		
		public string FileNamePattern
		{
			get { return "*.txt"; }
		}
		
		public bool FileNameMatches(string fileName)
		{
			return fileName.EndsWith(".txt");
		}
		
		
		public Image Icon
		{
			get { return Image.FromFile("../../../../Resources/Icons/text.png"); }
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
			get { return false; }
		}				
		
		public void Execute(string content, TextWriter stdout)
		{
			
		}			
	}
}
