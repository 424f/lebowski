using System;
using TwinEditor.FileTypes;

namespace TwinEditor.UI
{
    public interface IApplicationView
    {
        IFileType[] FileTypes { get; set; }
        
        void Show();
    }
}
