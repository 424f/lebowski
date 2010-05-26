namespace Lebowski.Net.Multichannel
{
    using System;
    using System.Collections.Generic;
    using log4net;
    
    /// <summary>
    /// Allows multiple separate connection to be tunneled
    /// through the same `IConnection`, by creating multiple channels.
    /// This is useful when you want to use the same connection for
    /// the synchroniation strategy, chat, execution result sharing, etc.
    /// </summary>
    public class MultichannelConnection
    {
        private static ILog Logger = LogManager.GetLogger(typeof(MultichannelConnection));

        private IConnection Connection;
        private Dictionary<int, TunneledConnection> channels;
        private Dictionary<TunneledConnection, int> channelIds;
        
        /// <summary>
        /// Initializes a new instance of the MultichannelConnection class. It
        /// takes a connection (which must not be used in any other context) and
        /// adds another layer on top of it, allowing multiple logically separate
        /// connections to be tunneled through it at the same time.
        /// </summary>
        /// <param name="connection"></param>
        public MultichannelConnection(IConnection connection)
        {
            Connection = connection;
            channels = new Dictionary<int, TunneledConnection>();
            channelIds = new Dictionary<TunneledConnection, int>();
            connection.Received += ConnectionReceived;
        }        

        /// <summary>
        /// Creates a new channeled connection.
        /// </summary>
        /// <remarks>
        /// As channels acquire their identifier automatically, CreateChannel()
        /// must be called in the same sequence at all sites, as to ensure
        /// that local channels correspond to the correct remote channels.
        /// </remarks>
        /// <returns>The channeled connection.</returns>
        public IConnection CreateChannel()
        {
            int channelId = channels.Count;
            var channel = new TunneledConnection(this);
            channels[channelId] = channel;
            channelIds[channel] = channelId;
            return channel;
        }

        /// <inheritdoc/>
        public void Close()
        {
            Connection.Close();
        }        
        
        internal void Send(TunneledConnection channel, object o)
        {
            int id = channelIds[channel];
            Connection.Send(new MultichannelMessage(id, o));
        }

        void ConnectionReceived(object sender, ReceivedEventArgs e)
        {
            MultichannelMessage msg;
            try
            {
                msg = (MultichannelMessage)e.Message;
            }
            catch(Exception exception)
            {
                Logger.ErrorFormat("Received invalid multichannel message: {0}", exception);
                return;
            }

            if (!channels.ContainsKey(msg.ChannelId))
            {
                Logger.Error(String.Format("Received message of type {0} on unknown channel {1} -- Will be ignored", msg.Message.GetType(), msg.ChannelId));
                return;
            }

            TunneledConnection channel = channels[msg.ChannelId];
            channel.OnReceived(new ReceivedEventArgs(msg.Message));
        }
    }
}