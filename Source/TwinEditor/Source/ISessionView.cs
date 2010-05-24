using System;
using Lebowski;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;
using Lebowski.TextModel;
using TwinEditor.FileTypes;
using log4net;

namespace TwinEditor
{
    public interface ISessionView
    {
        string FileName { get; set; }
        SessionContext SessionContext { get; }
    }
}
