
namespace Lebowski.Net.Skype
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Threading;
    using SKYPE4COMLib;
    using Lebowski.Net;
    using log4net;
    public sealed class SkypeConnection : AbstractConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));

        private string remote;
        public int IncomingChannel { get; private set; }
        public int OutgoingChannel { get; set; }

        private SkypeProtocol protocol;

        private bool dispatcherRunning;

        private Queue<ReceivedEventArgs> receiveQueue = new Queue<ReceivedEventArgs>();

        public SkypeConnection(SkypeProtocol protocol, string remote, int incomingChannel)
        {
            this.IncomingChannel = incomingChannel;
            this.protocol = protocol;
            this.remote = remote;
            OutgoingChannel = -1;

            // Start a dispatch thread (TODO: close again after connection not used anymore)
            ThreadStart threadStart = new ThreadStart(RunDispatcherThread);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        public override void Send(object o)
        {
            if (OutgoingChannel == -1)
            {
                throw new InvalidOperationException("You have to define an outgoing channel before sending messages.");
            }
            protocol.Send(remote, OutgoingChannel, o);
        }

        internal void ReceiveMessage(ReceivedEventArgs e)
        {
            // To avoid strange skype behavior, we don't immediately dispatch
            lock(receiveQueue)
            {
                receiveQueue.Enqueue(e);
                Monitor.PulseAll(receiveQueue);
            }
        }

        private void RunDispatcherThread()
        {
            dispatcherRunning = true;
            while (dispatcherRunning)
            {
                ReceivedEventArgs e;
                lock(receiveQueue)
                {
                    while (receiveQueue.Count == 0 && dispatcherRunning)
                    {
                        Monitor.Wait(receiveQueue);
                    }
                    e = receiveQueue.Dequeue();
                }
                OnReceived(e);
            }
        }

        public override void Close()
        {
            dispatcherRunning = false;
            lock(receiveQueue)
            {
                Monitor.Pulse(receiveQueue);
            }
        }

    }
}