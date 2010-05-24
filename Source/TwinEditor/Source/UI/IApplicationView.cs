using System;
using TwinEditor.FileTypes;
using Lebowski;
using Lebowski.Net;

namespace TwinEditor.UI
{
    public interface IApplicationView
    {
        IFileType[] FileTypes { get; set; }
        ICommunicationProtocol[] CommunicationProtocols { get; set; }
        ISessionContext CreateNewSession(IFileType fileType);
        
        void Show();
    }
}
