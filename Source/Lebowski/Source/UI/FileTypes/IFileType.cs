using System;
using System.Drawing;

namespace Lebowski.UI.FileTypes
{
	public interface IFileType
	{
		string Name { get; }
		string FileNamePattern { get; }
		Image Icon { get; }
	}
}
