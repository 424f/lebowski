namespace Lebowski.Net.Local
{
    using System;
    using System.Threading;
    using System.Collections.Generic;    
    
    /// <summary>
    /// Provides functionality to allow local connecctions
    /// (i.e. within the application), mainly for testing scenarios.
    /// </summary>
    public class LocalConnection : AbstractConnection
    {
        private Thread dispatcherThread;
        private Queue<ReceivedEventArgs> eventQueue;
        private bool dispatcherRunning;
        private bool paused;
        
        /// <summary>
        /// Initializes a new instance of the LocalConnection class.
        /// </summary>
        public LocalConnection()
        {
            eventQueue = new Queue<ReceivedEventArgs>();

            // Start dispatcher thread
            ThreadStart threadStart = new ThreadStart(RunDispatcher);
            dispatcherThread = new Thread(threadStart);
            dispatcherThread.Name = "LocalConnection Dispatcher Thread";
            dispatcherThread.Start();
        }
        
        /// <summary>
        /// Sets or gets the endpoint of the local connection.
        /// </summary>
        public LocalConnection Endpoint { set; protected get; }
        
        /// <inheritdoc/>
        public override void Send(object message)
        {
            System.Console.WriteLine("Send({0})", message);
            lock (eventQueue)
            {
                eventQueue.Enqueue(new ReceivedEventArgs(message));
                Monitor.PulseAll(eventQueue);
            }
        }

        /// <inheritdoc/>
        public override void Close()
        {
            dispatcherRunning = false;
            OnConnectionClosed(new EventArgs());
        }
        
        /// <summary>
        /// Pauses the sending of messages.
        /// </summary>
        public void PauseSending()
        {
            paused = true;
        }

        /// <summary>
        /// Resume the sending of messages.
        /// </summary>
        public void ResumeSending()
        {
            paused = false;
            lock (eventQueue)
            {
                Monitor.PulseAll(eventQueue);
            }            
        }        
        
        /// <summary>
        /// Makes sure that all packets currently enqueued are dispatched,
        /// before the method returns. This is handy for writing tests, as
        /// we have to wait until the packets have actually been dispatched
        /// before we look at the context states.
        /// </summary>
        public void DispatchAll()
        {
            lock (eventQueue)
            {
                while (eventQueue.Count > 0)
                {
                    Monitor.Wait(eventQueue);
                }
            }
        }
        
        /// <summary>
        /// Runs the dispatcher thread, waiting for packets to arrive and
        /// dispatching them one after another.
        /// </summary>
        private void RunDispatcher()
        {
            dispatcherRunning = true;
            while (dispatcherRunning)
            {
                ReceivedEventArgs e = null;
                lock (eventQueue)
                {
                    while (eventQueue.Count == 0 || paused)
                    {
                        Monitor.Wait(eventQueue);
                    }
                    e = eventQueue.Dequeue();
                }
                Endpoint.OnReceived(e);
                lock (eventQueue)
                {
                    Monitor.PulseAll(eventQueue);
                }
            }
        }        
    }
}
