using System.Collections.Generic;

namespace TwinEditor.UI
{
    using System;    
    using Lebowski;
    using Lebowski.Net;
    using TwinEditor.FileTypes;
    using TwinEditor.Sharing;
    
    public interface IApplicationView
    {
        IFileType[] FileTypes { get; set; }
        ICommunicationProtocol[] CommunicationProtocols { get; set; }
        ISessionView CreateNewSession(IFileType fileType);

        void UpdateRecentFiles(List<String> recentFiles);
        void Show();

        ApplicationContext ApplicationContext { get; set; }

        #region Events

        event EventHandler<SaveEventArgs> Save;
        event EventHandler<OpenEventArgs> Open;
        event EventHandler<CloseEventArgs> Close;
        event EventHandler<ShareSessionEventArgs> ShareSession;

        #endregion
    }

    public sealed class ShareSessionEventArgs : EventArgs
    {
        public ICommunicationProtocol Protocol { get; private set; }
        public SessionContext Session { get; private set; }

        public ShareSessionEventArgs(SessionContext session, ICommunicationProtocol protocol)
        {
            Protocol = protocol;
            Session = session;
        }
    }

    public sealed class OpenEventArgs : EventArgs
    {
        public IFileType FileType { get; private set; }
        public string FileName { get; private set; }

        public OpenEventArgs(string fileName, IFileType fileType)
        {
            FileName = fileName;
            FileType = fileType;
        }
    }

    public sealed class CloseEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        
        public CloseEventArgs(string fileName)
        {
            FileName = fileName;   
        }
    }
    
    public sealed class SaveEventArgs : EventArgs
    {
        public ISessionView Session { get; private set; }
        public string FileName { get; private set; }

        public SaveEventArgs(ISessionView session, string fileName)
        {
            Session = session;
            FileName = fileName;
        }
    }
}