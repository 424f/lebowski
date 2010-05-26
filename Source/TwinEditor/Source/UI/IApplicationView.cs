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
        ApplicationContext ApplicationContext { get; set; }
        
        void UpdateRecentFiles(List<String> recentFiles);
        void Show();
        void DisplayError(string message, Exception exception);

        event EventHandler<SaveFileEventArgs> SaveFile;
        event EventHandler<OpenFileEventArgs> OpenFile;
        event EventHandler<CloseFileEventArgs> CloseFile;
        event EventHandler<NewFileEventArgs> NewFile;
        event EventHandler<ShareSessionEventArgs> ShareSession;
        event EventHandler<EventArgs> ApplicationClosing;
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

    public sealed class OpenFileEventArgs : EventArgs
    {
        public IFileType FileType { get; private set; }
        public string FileName { get; private set; }

        public OpenFileEventArgs(string fileName, IFileType fileType)
        {
            FileName = fileName;
            FileType = fileType;
        }
    }

    public sealed class CloseFileEventArgs : EventArgs
    {
        public SessionContext Session { get; private set; }
        
        public CloseFileEventArgs(SessionContext session)
        {
            Session = session;
        }
    }
    
    public sealed class SaveFileEventArgs : EventArgs
    {
        public SaveFileEventArgs(SessionContext session, string fileName)
        {
            Session = session;
            FileName = fileName;
        }
        
        public SessionContext Session { get; private set; }
        public string FileName { get; private set; }        
    }
    
    public sealed class NewFileEventArgs : EventArgs
    {
        public NewFileEventArgs(IFileType fileType)
        {
            FileType = fileType;
        }
        
        public IFileType FileType { get; private set; }        
    }    
}