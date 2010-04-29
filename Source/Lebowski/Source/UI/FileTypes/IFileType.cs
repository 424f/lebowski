using System;
using System.Drawing;

namespace Lebowski.UI.FileTypes
{
	public interface IFileType
	{
		/// <summary>
		/// A descriptive name that will be used to display this file type
		/// to the user in the user interface
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// The pattern for files that are of this type.
		/// See http://msdn.microsoft.com/en-us/library/system.windows.controls.openfiledialog.filter(VS.95).aspx
		/// for the needed form.
		/// </summary>
		string FileNamePattern { get; }
		
		/// <summary>
		/// Checks whether `fileName` appears to be the name of a file
		/// of this type.
		/// </summary>
		bool FileNameMatches(string fileName);		
		
		/// <summary>
		/// The icon that is associated with this type and is displayed
		/// in the GUI (e.g. create file dialog, tab page, ...)
		/// </summary>
		Image Icon { get; }
	}
}
