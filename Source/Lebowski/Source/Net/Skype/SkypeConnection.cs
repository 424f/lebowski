namespace Lebowski.Net.Skype
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Threading;
    using SKYPE4COMLib;
    using Lebowski.Net;
    using log4net;
    
    /// <summary>
    /// Represents a connection that uses the Skype AP2AP API.
    /// 
    /// <remarks>As we have only one stream per user, we route messages
    /// through the parent SkypeProtocol. We use a different channel
    /// for each SkypeConnection, which is represented by an integer (unique
    /// on a per-user level).
    /// </remarks>
    /// </summary>
    public sealed class SkypeConnection : AbstractConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
        
        /// <summary>
        /// The username of the remote skype user.
        /// </summary>
        private string remote;
        
        /// <summary>
        /// The SkypeProtocol over which we will send messages.
        /// </summary>
        private SkypeProtocol protocol;
        
        /// <summary>
        /// Indicates whether the dispatcher thread should keep running.
        /// </summary>
        private bool dispatcherRunning;
        
        /// <summary>
        /// The message queue from which the dispatcher thread dispatches messages.
        /// </summary>
        private Queue<ReceivedEventArgs> receiveQueue = new Queue<ReceivedEventArgs>();

        /// <summary>
        /// Initializes a new instance of the SkypeConnection class, given a 
        /// Skype protocol, the name of a skype friend and the incoming channel
        /// to be used.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="remote"></param>
        /// <param name="incomingChannel"></param>
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

        /// <summary>
        /// Identifier of the channel within SkypeProtocol over which incoming
        /// messages are received.
        /// </summary>
        public int IncomingChannel { get; private set; }

        /// <summary>
        /// Identifier of the channel within SkypeProtocol over which outgoing
        /// messages are sent.
        /// </summary>        
        public int OutgoingChannel { get; internal set; }

        /// <inheritdoc/>
        public override void Close()
        {
            dispatcherRunning = false;
            lock(receiveQueue)
            {
                Monitor.Pulse(receiveQueue);
            }
        }        
        
        internal void ReceiveMessage(ReceivedEventArgs e)
        {
            // To avoid strange skype behavior which result in hard to understand
            // call stacks, we don't immediately dispatch, but use a dispatcher
            // thread with its own stack frame.
            lock(receiveQueue)
            {
                receiveQueue.Enqueue(e);
                Monitor.PulseAll(receiveQueue);
            }
        }

        /// <summary>
        /// Runs a dispatcher thread, which dispatches messages from the 
        /// receiveQueue until the connection has been closed.
        /// </summary>
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
        
        /// <inheritdoc/>
        public override void Send(object o)
        {
            if (OutgoingChannel == -1)
            {
                throw new InvalidOperationException("You have to define an outgoing channel before sending messages.");
            }
            protocol.Send(remote, OutgoingChannel, o);
        }
    }
}