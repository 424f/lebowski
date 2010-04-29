﻿using System;
using System.Drawing;

namespace Lebowski.UI.FileTypes
{
	public class PythonFileType : IFileType
	{
		public string Name
		{
			get { return "Python"; }
		}
		
		public string FileNamePattern
		{
			get { return "*.py"; }
		}
		
		public Image Icon
		{
			get { return Image.FromFile("../../../../Resources/Icons/python.png"); }
		}
		
	}
}