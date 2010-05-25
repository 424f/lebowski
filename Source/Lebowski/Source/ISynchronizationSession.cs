namespace Lebowski
{
    using System;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using Lebowski.Net;    
    
    /// <summary>
    /// Describes the part of the state of a synchronization session
    /// that is relevant for a synchronization algorithm to perform
    /// its task.
    /// </summary>
    public interface ISynchronizationSession
    {
        /// <summary>
        /// The state this session currently is in
        /// </summary>
        SessionStates State { get; set; }
        
        /// <summary>
        /// The synchronization that is used to keep the session in a consistent
        /// state at all sites at all times.
        /// </summary>
        DifferentialSynchronizationStrategy SynchronizationStrategy { get; }
        
        /// <summary>
        /// The text context at the current site
        /// </summary>
        TextModel.ITextContext Context { get; }
        
        /// <summary>
        /// The file name that is associated with the session at the current 
        /// site (if any)
        /// </summary>
        string FileName { get; set; }
            
    }
}
