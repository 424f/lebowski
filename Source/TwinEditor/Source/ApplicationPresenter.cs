using System;
using System.Linq;
using Lebowski;
using TwinEditor.UI;
using TwinEditor.FileTypes;

namespace TwinEditor
{
	public class ApplicationPresenter
	{
		public ApplicationPresenter(IApplicationView view)
		{
            // Provide view with file types
			var fileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType))
				.Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
				.Cast<IFileType>()
				.ToArray();
            view.FileTypes = fileTypes;
		}
	}
}
