using System;
using Lebowski.Synchronization.DifferentialSynchronization;
using Lebowski.Net;

namespace Lebowski
{
    public interface ISynchronizationSession
    {
        SessionStates State { get; set; }
        DifferentialSynchronizationStrategy SynchronizationStrategy { get; }
        TextModel.ITextContext Context { get; }
        
        string FileName { get; set; }
            
    }
}
