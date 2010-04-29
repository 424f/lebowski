using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Lebowski.Synchronization.DifferentialSynchronization;
using SKYPE4COMLib;
using Lebowski.Net;
using log4net;

// TODO: remove dependencies on specific synchronization strategy

namespace Lebowski.Net.Skype
{
	sealed class SharingInvitationMessage
	{
		public string DocumentName { get; private set; }
		
		public SharingInvitationMessage(string documentName)
		{
			DocumentName = documentName;
		}
	}
	
	public class SkypeProtocol : ICommunicationProtocol
	{
		private const int ApplicationConnectionId = 0;
		
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		private const string ApplicationName = "LEBOWSKI-01";
		private Application Application;
			
		private SKYPE4COMLib.Skype API;
		
		private bool isInitialized = false;
		
		public List<string> friends = new List<string>();
		
		/// <summary>
		/// We keep only one stream per user, even if we're sharing multiple documents
		/// or in multiple directions. To enable this, we prepend the connection id for
		/// every packet that is sent over a stream. The connectionId 0 is reserver for 
		/// application-wide messages (e.g. invitations)
		/// </summary>
		public Dictionary<string, ApplicationStream> streams = new Dictionary<string, ApplicationStream>();
		public Dictionary<string, Dictionary<int, SkypeConnection>> connections = new Dictionary<string, Dictionary<int, SkypeConnection>>();
		public Dictionary<string, int> connectionsForUser = new Dictionary<string, int>();
		
		public SkypeProtocol()
		{
			Initialize();

			/*string user = API.CurrentUser.FullName;*/
			
			UpdateFriends();
			
			//Application.Connect(username, true);
			//Application.ConnectableUsers.Add(username);
		}
		
		public void EstablishConnection(string user)
		{
			// We might have to create a stream to this user
			if(!streams.ContainsKey(user))
			{
				Application.Connect(user, true);
			}

			while(!streams.ContainsKey(user))
			{
				// TODO: is wait necessary? if yes, use Monitor
				Thread.Sleep(100);
			}			
		}
		
		public SkypeConnection Connect(string user)
		{
			EstablishConnection(user);
				
			connectionsForUser[user] = connectionsForUser.ContainsKey(user) ? connectionsForUser[user]+1 : 1;
			Console.WriteLine("Creating connection {0} for user {1}", connectionsForUser[user], user);
			SkypeConnection connection = new SkypeConnection(this, user, connectionsForUser[user]);
			
			return connection;
		}
		
		protected void UpdateFriends()
		{
			this.friends.Clear();
			
			var friends = API.Friends;
			
			for(int i = 1; i <= friends.Count; ++i)
			{
				var friend = friends[i];
				if(friend.OnlineStatus != TOnlineStatus.olsOffline)
				{
					Console.WriteLine(friend.FullName + " / " + friend.Handle + " / " + friend.DisplayName + " / " + friend.OnlineStatus);
				}
				this.friends.Add(friend.Handle);
			}			
			
		}
		
		public string Name {
			get { return "Skype API"; }
		}
		
		public void Initialize()
		{
			if(isInitialized)
				return;
			
			API = new SKYPE4COMLib.Skype();
			API.Attach(8, true);
			Application = API.get_Application(ApplicationName);
			Application.Create();	
			
			Console.WriteLine("Application created..");
						
			API.ApplicationStreams += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{
				if(pApp.Name != Application.Name)
					return;
				
				Console.Write("Application stream: ");
				for(int i = 1; i <= pStreams.Count; ++i)
				{
					Console.Write("{0} {1} | ", pStreams[i].Handle, pStreams[i].PartnerHandle);
				}
				Console.WriteLine();
				
				if(streams.ContainsKey(pStreams[1].PartnerHandle))
				{
					throw new Exception("Already a stream available for " + pStreams[1].PartnerHandle);
				}
				streams[pStreams[1].PartnerHandle] = pStreams[1];				
				connections[pStreams[1].PartnerHandle] = new Dictionary<int, SkypeConnection>();
			};
			
			API.ApplicationConnecting += delegate(Application pApp, UserCollection pUsers)
			{
				Console.Write("Connecting: " + pApp.Name + ":: ");
				for(int i = 1; i <= pUsers.Count; ++i)
				{
					Console.Write(pUsers[i].Handle + " ");
				}
				Console.WriteLine();
				
				
				if(pUsers.Count == 1)
				{
					Console.WriteLine("Connecting...");
				}
				
				if(pUsers.Count == 0)
				{
					Console.WriteLine("Waiting for accept...");
				}
			};
						
			
			API.ApplicationReceiving += delegate(Application pApp, ApplicationStreamCollection pStreams)
			{ 
				Console.Write("Receving: {0} ::", pApp.Name);
				for(int i = 1; i <= pStreams.Count; ++i)
				{
					Console.Write("{0} ", pStreams[i].Handle);
				}
				Console.WriteLine();
				
				if(pStreams.Count == 0)
					return;
				if(pStreams[1].DataLength == 0)
					return;
				string text = pStreams[1].Read();
				Console.WriteLine("RECV " + text);
				
				//stream.Write("hey dude");
				
				// Decode message
				Logger.Info(string.Format("Received skype datagram: {0}", text));
				try
				{
					byte[] both = Convert.FromBase64String(text);
					int connectionId = BitConverter.ToInt32(both, 0);

					object message = NetUtils.Deserialize(both, 4, both.Length-4);
					
					if(connectionId == ApplicationConnectionId)
					{
						if(message is SharingInvitationMessage)
						{
							SharingInvitationMessage sharingInvitation = (SharingInvitationMessage)message;
							System.Windows.Forms.MessageBox.Show("Received invitation!!");
						}
					}
					else
					{
						connections[pStreams[1].PartnerHandle][connectionId].OnReceived(new ReceivedEventArgs(message));
					}
				}
				catch(Exception e)
				{
					Logger.Error("Received ill-formed skype datagram", e);
				}				
				
			};						
			
			isInitialized = true;

		}
		
		public void Share(ISessionContext session)
		{
			Initialize();
			SkypeShareForm form = new SkypeShareForm(this);
			form.Submit += delegate
			{  
				EstablishConnection(form.SelectedUser);
				Send(form.SelectedUser, ApplicationConnectionId, new SharingInvitationMessage("fooo"));
				
				/*// Send an invitation
				SkypeConnection connection = Connect(form.SelectedUser);
				
				// We have to use a multichannel connection
				MultichannelConnection mcc = new MultichannelConnection(connection);
				var sync = new DifferentialSynchronizationStrategy(0, session.Context, mcc.CreateChannel());
				var applicationChannel = mcc.CreateChannel();		
				session.StartSession(sync, connection, applicationChannel);	*/			
			};
			form.ShowDialog();
		}
		
		public bool CanShare
		{
			get { return true; }
		}
		
		public bool CanParticipate
		{
			get { return true; }
		}		
		
		internal void Send(string user, int connectionId, object o)
		{
			var stream = streams[user];
			
			byte[] prefix = BitConverter.GetBytes(connectionId);
			byte[] buffer = NetUtils.Serialize(o);
			
			byte[] both = new Byte[prefix.Length + buffer.Length];
			prefix.CopyTo(both, 0);
			buffer.CopyTo(both, prefix.Length);
			
			string base64buffer = Convert.ToBase64String(both);
			
			stream.Write(base64buffer);			
		}
	}
}
