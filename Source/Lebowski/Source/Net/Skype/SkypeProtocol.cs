﻿using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Lebowski.Synchronization.DifferentialSynchronization;
using SKYPE4COMLib;
using System.Windows.Forms;
using Lebowski.Net;
using log4net;

// TODO: remove dependencies on specific synchronization strategy

namespace Lebowski.Net.Skype
{
	[Serializable]
	sealed class SharingInvitationMessage
	{
		/// <summary>
		/// An identifier uniquely identifying the invitation on the host side
		/// </summary>
		public int InvitationId { get; private set; }
		
		/// <summary>
		/// The name of the document that the host would like to share
		/// </summary>
		public string DocumentName { get; private set; }
		
		/// <summary>
		/// The name of the invited user
		/// </summary>
		public string InvitedUser { get; private set; }
		
		/// <summary>
		/// The channel, i.e. the connectionId to we, after the session is established, should send our
		/// data
		/// </summary>
		public int Channel { get; private set; }
		
		public SharingInvitationMessage(int invitationId, string documentName, string invitedUser, int channel)
		{
			InvitationId = invitationId;
			DocumentName = documentName;
			InvitedUser = invitedUser;
			Channel = channel;
		}
	}
	
	[Serializable]
	sealed class DeclineSharingInvitationMessage
	{
		public int InvitationId { get; private set; }
		
		public DeclineSharingInvitationMessage(int invitationId)
		{
			InvitationId = invitationId;
		}
	}	

	[Serializable]
	sealed class AcceptSharingInvitationMessage
	{
		public int InvitationId { get; private set; }
		public int Channel { get; private set; }
		
		public AcceptSharingInvitationMessage(int invitationId, int channel)
		{
			InvitationId = invitationId;
			Channel = channel;
		}
	}		
	
	sealed public class SkypeProtocol : ICommunicationProtocol
	{
		public event EventHandler<HostSessionEventArgs> HostSession;
		public event EventHandler<JoinSessionEventArgs> JoinSession;
		
		private const int ApplicationConnectionId = 0;
		
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));
		
		private const string ApplicationName = "LEBOWSKI-01";
		private SKYPE4COMLib.Application Application;
			
		private SKYPE4COMLib.Skype API;
		
		private bool isInitialized = false;
		
		public List<string> friends = new List<string>();
		
		/// <summary>
		/// We keep only one stream per user, even if we're sharing multiple documents
		/// or in multiple directions. To enable this, we prepend the connection id for
		/// every packet that is sent over a stream. The connectionId 0 is reserver for 
		/// application-wide messages (e.g. invitations)
		/// </summary>
		Dictionary<string, ApplicationStream> streams = new Dictionary<string, ApplicationStream>();
		Dictionary<string, Dictionary<int, SkypeConnection>> connections = new Dictionary<string, Dictionary<int, SkypeConnection>>();
		Dictionary<string, int> connectionsForUser = new Dictionary<string, int>();
		
		Dictionary<int, SharingInvitationMessage> invitations = new Dictionary<int, SharingInvitationMessage>();
		Dictionary<int, SkypeConnection> invitationChannels = new Dictionary<int, SkypeConnection>();
		Dictionary<int, ISessionContext> invitationSessions = new Dictionary<int, ISessionContext>();
		
		int numInvitations = 0;
		
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
		
		void UpdateFriends()
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
						
			API.ApplicationStreams += delegate(SKYPE4COMLib.Application pApp, ApplicationStreamCollection pStreams)
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
			
			API.ApplicationConnecting += delegate(SKYPE4COMLib.Application pApp, UserCollection pUsers)
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
						
			
			API.ApplicationReceiving += delegate(SKYPE4COMLib.Application pApp, ApplicationStreamCollection pStreams)
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
					
					string partner = pStreams[1].PartnerHandle;
					
					if(connectionId == ApplicationConnectionId)
					{
						// Received a sharing invitation:
						// the user might now either accept or reject the invitation
						// and we'll have to notify the host about his choice.
						if(message is SharingInvitationMessage)
						{
							SharingInvitationMessage sharingInvitation = (SharingInvitationMessage)message;
							text = string.Format("You received an invitation from {0} via Skype to share document {1}. Would you like to accept?", partner, sharingInvitation.DocumentName);
							var result = MessageBox.Show(text, "Invitation", MessageBoxButtons.YesNo);
							if(result == DialogResult.Yes)
							{
								SkypeConnection connection = Connect(partner);
								connection.OutgoingChannel = sharingInvitation.Channel;
								
								MultichannelConnection mcc = new MultichannelConnection(connection);
								OnJoinSession(new JoinSessionEventArgs(mcc.CreateChannel(), mcc.CreateChannel()));
								                                       
								Send(partner, 0, new AcceptSharingInvitationMessage(sharingInvitation.InvitationId, connection.IncomingChannel));
								//Send(partner, 0, new AcceptSharingInvitationMessage());
									
								// TODO: set up new editor for the session
							}
							else
							{
								Send(partner, 0, new DeclineSharingInvitationMessage(sharingInvitation.InvitationId));
							}
						}
						// Another user has accepted a sharing invitation
						else if(message is AcceptSharingInvitationMessage)
						{
							AcceptSharingInvitationMessage accept = (AcceptSharingInvitationMessage)message;
							if(!invitations.ContainsKey(accept.InvitationId))
							{
								Logger.Error(string.Format("Received {0} but no such invitation was sent.", message));
								return;
							}
							
							SharingInvitationMessage sharingInvitation = invitations[accept.InvitationId];
							if(sharingInvitation.InvitedUser != partner)
							{
								Logger.Error(string.Format("{0} accepted {1} intended for {2}", partner, sharingInvitation, sharingInvitation.InvitedUser));
							}
							
							IConnection connection = invitationChannels[accept.InvitationId];
							ISessionContext session = invitationSessions[accept.InvitationId];
							
							invitations.Remove(accept.InvitationId);
							invitationChannels.Remove(accept.InvitationId);
							invitationSessions.Remove(accept.InvitationId);
							
							MessageBox.Show("Invitation was accepted");
				
							// We have to use a multichannel connection
							
							MultichannelConnection mcc = new MultichannelConnection(connection);
							OnHostSession(new HostSessionEventArgs(session, mcc.CreateChannel(), mcc.CreateChannel()));
						}
						
						// Another user has rejected a sharing invitation
						else if(message is DeclineSharingInvitationMessage)
						{
							DeclineSharingInvitationMessage decline = (DeclineSharingInvitationMessage)message;
							if(!invitations.ContainsKey(decline.InvitationId))
							{
								Logger.Error(string.Format("Received {0} but no such invitation was sent.", message));
								return;
							}
							
							SharingInvitationMessage sharingInvitation = invitations[decline.InvitationId];
							if(sharingInvitation.InvitedUser != partner)
							{
								Logger.Error(string.Format("{0} rejected {1} intended for {2}", partner, sharingInvitation, sharingInvitation.InvitedUser));
							}
							
							invitations.Remove(decline.InvitationId);
							invitationChannels.Remove(decline.InvitationId);
							invitationSessions.Remove(decline.InvitationId);
							
							MessageBox.Show("Invitation was rejected");
						}						
					}
					else
					{
						connections[pStreams[1].PartnerHandle][connectionId].OnReceived(new ReceivedEventArgs(message));
					}
				}
				catch(Exception e)
				{
					Console.Error.WriteLine("Receive error: {0}", e);
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
				
				// Create channel for this session
				SkypeConnection connection = Connect(form.SelectedUser);
				
				// Send out the invite
				SharingInvitationMessage invitationMessage = new SharingInvitationMessage(++numInvitations, session.FileName, form.SelectedUser, connection.IncomingChannel);
				invitations[invitationMessage.InvitationId] = invitationMessage;
				invitationChannels[invitationMessage.InvitationId] = connection;
				invitationSessions[invitationMessage.InvitationId] = session;
				Send(form.SelectedUser, ApplicationConnectionId, invitationMessage);
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
		
		void OnHostSession(HostSessionEventArgs e)
		{
			if (HostSession != null) {
				HostSession(this, e);
			}
		}
		
		void OnJoinSession(JoinSessionEventArgs e)
		{
			if (JoinSession != null) {
				JoinSession(this, e);
			}
		}
		
	}
}
