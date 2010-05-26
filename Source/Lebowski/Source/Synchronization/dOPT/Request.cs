using System;

namespace Lebowski.Synchronization.dOPT
{
    public class Request<OperationType>
    {
        public int SiteId { get; protected set; }
        public StateVector State { get; protected set; }
        public OperationType Operation { get; protected set; }
        public int Priority { get; protected set; }

        public Request(int siteId, StateVector state, OperationType op, int priority)
        {
            SiteId = siteId;
            State = state;
            Operation = op;
            Priority = priority;
        }
    }
}
