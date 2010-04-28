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
		
		public Image Icon
		{
			get { return Image.FromFile("../../../../Resources/Icons/text.png"); }
		}
		
	}
}
