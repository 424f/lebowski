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
        ISessionView CreateNewSession(IFileType fileType);
        
        void Show();
        
        ApplicationPresenter Presenter { get; set; }
        
        event EventHandler<ShareSessionEventArgs> ShareSession;
        
        #region File handling
        
        event EventHandler<SaveEventArgs> Save;
        event EventHandler<OpenEventArgs> Open;
        
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
