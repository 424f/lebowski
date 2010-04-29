﻿using System;
using System.IO;
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
	
		public bool FileNameMatches(string fileName)
		{
			return fileName.EndsWith(".py");
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
			
		}		
	}
}
