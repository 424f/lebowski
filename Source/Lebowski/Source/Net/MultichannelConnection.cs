﻿using System;
using System.Collections.Generic;
using log4net;

namespace Lebowski.Net
{
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
		
		internal void Send(TunneledConnection channel, object o)
		{
			int id = channelIds[channel];
			Connection.Send(new MultichannelMessage(id, o));
		}
		
		void ConnectionReceived(object sender, ReceivedEventArgs e)
		{						
			MultichannelMessage msg = (MultichannelMessage)e.Message;
			
			if(!channels.ContainsKey(msg.ChannelId))
			{
				Logger.Error(String.Format("Received message of type {0} on unknown channel {1} -- Will be ignored", msg.Message.GetType(), msg.ChannelId));
				return;
			}
			   
			TunneledConnection channel = channels[msg.ChannelId];
			channel.OnReceived(new ReceivedEventArgs(msg.Message));
		}
		
		public MultichannelConnection(IConnection connection)
		{
			Connection = connection;
			channels = new Dictionary<int, TunneledConnection>();
			channelIds = new Dictionary<TunneledConnection, int>();
			connection.Received += ConnectionReceived;
		}
		
		public IConnection CreateChannel()
		{
			int channelId = channels.Count;
			var channel = new TunneledConnection(this);
			channels[channelId] = channel;
			channelIds[channel] = channelId;
			return channel;
		}
	}
	
	class TunneledConnection : IConnection
	{
		public event EventHandler<ReceivedEventArgs> Received;
		
		private MultichannelConnection tunnel;
		
		public TunneledConnection(MultichannelConnection tunnel)
		{
			this.tunnel = tunnel;
		}
		
		public void Send(object o)
		{
			tunnel.Send(this, o);
		}
		
		public void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
	}
	
	[Serializable]
	class MultichannelMessage
	{
		public int ChannelId { get; private set; }
		public object Message { get; private set; }
		
		public MultichannelMessage(int channelId, object message)
		{
			ChannelId = channelId;
			Message = message;
		}
	}
}