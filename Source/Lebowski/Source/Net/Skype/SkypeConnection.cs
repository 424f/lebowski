using System;
using System.Text;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

namespace Lebowski.Net.Skype
{
	public class SkypeConnection : IConnection
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		public event EventHandler<ReceivedEventArgs> Received;
		private Application application;
		private SKYPE4COMLib.Skype api;
		
		public SkypeConnection(SKYPE4COMLib.Skype api, Application application)
		{
			this.api = api;
			this.application = application;
			
			api.ApplicationDatagram += delegate(Application app, ApplicationStream stream, string text) { 
				if(app != this.application)
				{
					return;
				}
				
				// Decode message
				Logger.Info(string.Format("Received skype datagram: {0}", text));
				try
				{
					byte[] buffer = Convert.FromBase64String(text);
					OnReceived(new ReceivedEventArgs(NetUtils.Deserialize(buffer)));
				}
				catch(Exception e)
				{
					Logger.Error("Received ill-formed skype datagram", e);
				}
			};			
		}
		
		public void Send(object o)
		{
			byte[] buffer = NetUtils.Serialize(o);
			string base64buffer = Convert.ToBase64String(buffer);
			
			application.SendDatagram(base64buffer, application.Streams);
		}
		
		protected virtual void OnReceived(ReceivedEventArgs e)
		{
			if (Received != null) {
				Received(this, e);
			}
		}
		
	}
}
