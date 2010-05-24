using System;
using Lebowski;
using TwinEditor.FileTypes;

namespace TwinEditor
{
    public interface ISession : ISynchronizationSession
    {
        IFileType FileType { get; set; }
    }
}
