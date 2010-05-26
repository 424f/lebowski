namespace Lebowski.Synchronization.dOPT
{
    using System;

    /// <summary>
    /// A Request for an operation that should be performed at remote dOPT
    /// sites.
    /// </summary>
    public class Request<OperationType>
    {
        /// <summary>
        /// The identifier for the originating site.
        /// </summary>
        public int SiteId { get; protected set; }
        
        /// <summary>
        /// The state at the originating site when the request was sent.
        /// </summary>
        public StateVector State { get; protected set; }
        
        /// <summary>
        /// The operation that should be performed.
        /// </summary>
        public OperationType Operation { get; protected set; }
        
        /// <summary>
        /// The operation's priority.
        /// </summary>
        public int Priority { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the Request class.
        /// </summary>
        public Request(int siteId, StateVector state, OperationType op, int priority)
        {
            SiteId = siteId;
            State = state;
            Operation = op;
            Priority = priority;
        }
    }
}
