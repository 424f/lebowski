

namespace Lebowski.Synchronization.dOPT
{
    using System;
    using System.Collections.Generic;
    using Lebowski;
    using Lebowski.TextModel;
    using Lebowski.TextModel.Operations;
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

    public class Model<OperationType, ContextType>
        where OperationType : TextOperation
    {
        int SiteId;
        List<Request<OperationType>> Requests;
        List<Request<OperationType>> Broadcasts;
        List<Request<OperationType>> Log;
        public ContextType Context { get; protected set; }

        public event EventHandler Received;
        public event EventHandler Execute;

        public StateVector State
        {
            get { return state; }
            protected set {    state = value; }
        }
        private StateVector state;

        public Model(int siteId, ContextType context)
        {
            SiteId = siteId;
            Context = context;

            Requests = new List<Request<OperationType>>();
            Broadcasts = new List<Request<OperationType>>();
            Log = new List<Request<OperationType>>();
            State = new StateVector(2);

        }

        public void Generate(OperationType operation)
        {
            // TODO: priority
            int p = SiteId;
            Request<OperationType> req = new Request<OperationType>(SiteId, State, operation, p);
            Requests.Add(req);
            Broadcast(req);
            ExecuteRequests();
        }

        public void Receive(Request<OperationType> req)
        {
            Requests.Add(req);
            ExecuteRequests();
            Received(this, null);
        }

        public void Write(string text)
        {
            Console.WriteLine(new String('\t', 3*SiteId) + text);
        }

        public void ExecuteRequests()
        {
            List<Request<OperationType>> removeList = new List<Request<OperationType>>();
            foreach (Request<OperationType> req in Requests)
            {
                int j = req.SiteId;
                StateVector sj = req.State;
                OperationType oj = req.Operation;
                int pj = req.Priority;

                if (sj > State)
                {
                    Write(String.Format("Skip as {0} > {1}", sj, State));
                    continue;
                }

                removeList.Add(req);
                Write(String.Format("Original: {0}", oj));

                if (sj < State)
                {
                    // Find last log entry that fulfills condition
                    int pos = Log.Count-1;
                    while (pos >= 0 && Log[pos].State > sj)
                    {
                        pos -= 1;
                    }

                    for(; pos < Log.Count; ++pos)
                    {
                        Request<OperationType> reqK = Log[pos];
                        int k = reqK.SiteId;
                        StateVector sk = reqK.State;
                        OperationType ok = reqK.Operation;
                        int pk = reqK.Priority;

                        if (sj[k] <= sk[k])
                        {
                            throw new NotImplementedException();
                            /*// TODO: priority
                            oj = (OperationType)oj.Transform(ok);
                            if (oj == null)
                            {
                                break;
                            }*/
                        }
                    }
                }

                Write(String.Format("Perform {0}", oj));

                if (oj != null)
                {
                    // TODO: oj.Apply(Context);
                    throw new NotImplementedException();
                }

                Log.Add(new Request<OperationType>(j, State, oj, pj));
                state[j] += 1;


            }

            foreach (Request<OperationType> req in removeList)
            {
                Requests.Remove(req);
            }
            Execute(this, null);
        }

        void Broadcast(Request<OperationType> req)
        {
            Broadcasts.Add(req);
        }

        public void DeliverBroadcasts(Model<OperationType, ContextType>[] others)
        {
            foreach (Request<OperationType> req in Broadcasts)
            {
                foreach (var other in others)
                {
                    other.Receive(req);
                }
            }
            Broadcasts.Clear();
        }
    }
}