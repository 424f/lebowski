using System;
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
		
	}
}
