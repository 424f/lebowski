namespace TwinEditor.Sharing
{
    using System;
    using Lebowski;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;
    using Lebowski.TextModel;
    using TwinEditor.FileTypes;
    using log4net;
    
    public interface ISessionView
    {
        string FileName { get; set; }
        SessionContext SessionContext { get; }
        
        bool OnDisk { get; set; }
        bool FileModified { get; set; }
    }
}