namespace Lebowski.Net.Skype
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Collections.Generic;
    using SKYPE4COMLib;
    using System.Windows.Forms;
    using Lebowski.Net;
    using Lebowski.Synchronization;
    using log4net;
    
    /// <summary>
    /// This provides a wrapper around the AP2AP Skype4COM API that allows us
    /// to send arbitrary objects to other skype users, even
    /// when behind a firewall. There currently are a few shortcomings:
    ///     - no proper handling of case when a user is logged into Skype
    ///       multiple times
    ///     - Objects are being serialized and then encoded to base64, as skype
    ///       does not allow '\0' in the data stream. This could be addressed
    ///       by using a more efficient encoding. Also, it might be worth
    ///       to look into ProtocolBuffers instead of using serialization.
    ///     - Skype does seem to produce weird call stacks, as calling a skype
    ///       API method can trigger a Skype event being triggered before that
    ///       original call returns. We addressed this by introducing a separate
    ///       dispatch thread.
    /// </summary>
    sealed public class SkypeProtocol : ICommunicationProtocol
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SkypeProtocol));        
        
        private const int ApplicationConnectionId = 0;
        private const string ApplicationName = "LEBOWSKI-01";
        private SKYPE4COMLib.Application Application;
        private SKYPE4COMLib.SkypeClass API;
        private bool isInitialized = false;
        private SynchronizationContext synchronizationContext;
        
        /// <summary>
        /// Gets all the friends that are online in the local user's friend list
        /// </summary>
        private Dictionary<string, User> friends = new Dictionary<string, User>();        
        
        /// <summary>
        /// We keep only one stream per user, even if we're sharing multiple documents
        /// or in multiple directions. To enable this, we prepend the connection id for
        /// every packet that is sent over a stream. The connectionId 0 is reserved for
        /// application-wide messages (e.g. invitations)
        /// </summary>
        private Dictionary<string, ApplicationStream> streams = new Dictionary<string, ApplicationStream>();
        
        private Dictionary<string, Dictionary<int, SkypeConnection>> connections = new Dictionary<string, Dictionary<int, SkypeConnection>>();
        private Dictionary<string, int> connectionsForUser = new Dictionary<string, int>();

        private Dictionary<int, SharingInvitationMessage> invitations = new Dictionary<int, SharingInvitationMessage>();
        private Dictionary<int, SkypeConnection> invitationChannels = new Dictionary<int, SkypeConnection>();
        private Dictionary<int, ISynchronizationSession> invitationSessions = new Dictionary<int, ISynchronizationSession>();

        private int numInvitations = 0;        
        
        /// <summary>
        /// Initializes a new instance of the SkypeProtocol class.
        /// </summary>
        public SkypeProtocol()
        {
            Enabled = true;

            try
            {
                API = new SKYPE4COMLib.SkypeClass();
                API.Attach(8, false);

                API._ISkypeEvents_Event_AttachmentStatus += delegate(TAttachmentStatus status)
                {
                    if (status == TAttachmentStatus.apiAttachSuccess)
                    {
                        Initialize();
                    }
                };
            }
            catch(Exception e)
            {
                Enabled = false;
                Logger.WarnFormat("Could not initialize {0}:\n{1}", GetType().Name, e.ToString());
            }
        }        

        /// <inheritdoc/>
        public bool CanShare
        {
            get { return true; }
        }

        /// <inheritdoc/>
        public bool CanParticipate
        {
            // When using skype we're actively sending invitations, so there is no way to manually participate in a session
            get { return false; }
        }        
        
        /// <inheritdoc/>
        public bool Enabled { get; private set; }        
        
        /// <summary>
        /// Gets all the friends that are online in the local user's friend list
        /// </summary>
        public List<string> FriendNames
        {
            get
            {
                List<string> result = new List<string>();
                foreach(User user in friends.Values)
                {
                    result.Add(user.Handle);
                }
                return result;
            }
        }        
        
        /// <summary>
        /// Checks whether the friend with the given username is online.
        /// </summary>
        /// <param name="userName">The user name of a friend.</param>
        /// <returns></returns>
        public bool IsUserOnline(string userName)
        {
            if(!friends.ContainsKey(userName))
                return false;
            return friends[userName].OnlineStatus != TOnlineStatus.olsUnknown &&
                   friends[userName].OnlineStatus != TOnlineStatus.olsOffline;
        }

        /// <inheritdoc/>
        public string Name
        {
            get { return "Skype API"; }
        }        

        /// <summary>
        /// Ensures that a connection stream is available to the specified user.
        /// </summary>
        /// <param name="user">The user to establish a connection to.</param>
        private void EstablishConnection(string user)
        {
            // We might have to create a stream to this user
            if (!streams.ContainsKey(user))
            {
                Application.Connect(user, true);
            }

            while (!streams.ContainsKey(user))
            {
                // TODO: is wait necessary? if yes, use Monitor
                Thread.Sleep(100);
            }
        }
        
        /// <summary>
        /// Initializes the protocol, by setting up event handlers and registering
        /// our application.
        /// </summary>
        private void Initialize()
        {
            if (isInitialized)
                return;

            synchronizationContext = System.Threading.SynchronizationContext.Current;

            Application = API.get_Application(ApplicationName);
            Application.Create();

            API._ISkypeEvents_Event_ApplicationStreams += ApplicationStreams;
            API.ApplicationConnecting += ApplicationConnecting;
            API.ApplicationReceiving += ApplicationReceiving;
            

            isInitialized = true;
            
            Logger.Info("Initialized SkypeProtocol");

        }        

        /// <summary>
        /// Establishes a connection to a user and returns the associated connection.
        /// </summary>
        /// <returns>The connection that can be used to communicate with this user.</returns>
        private SkypeConnection Connect(string user)
        {
            EstablishConnection(user);

            connectionsForUser[user] = connectionsForUser.ContainsKey(user) ? connectionsForUser[user]+1 : 1;
            Logger.InfoFormat("Creating connection {0} for user {1}", connectionsForUser[user], user);
            SkypeConnection connection = new SkypeConnection(this, user, connectionsForUser[user]);
            connections[user][connectionsForUser[user]] = connection;

            return connection;
        }

        /// <inheritdoc/>
        public void Share(ISynchronizationSession session)
        {
            UpdateFriends();
            SkypeShareForm form = new SkypeShareForm(this);
            form.Submit += delegate
            {
                EstablishConnection(form.SelectedUser);

                // TODO: connect in separate thread

                // Create channel for this session
                SkypeConnection connection = Connect(form.SelectedUser);

                // Send out the invite
                SharingInvitationMessage invitationMessage = new SharingInvitationMessage(++numInvitations, session.FileName, form.SelectedUser, connection.IncomingChannel);
                invitations[invitationMessage.InvitationId] = invitationMessage;
                invitationChannels[invitationMessage.InvitationId] = connection;
                invitationSessions[invitationMessage.InvitationId] = session;
                Send(form.SelectedUser, ApplicationConnectionId, invitationMessage);

                session.State = SessionStates.AwaitingConnection;

                form.Close();
            };
            form.ShowDialog();
        }
        
        /// <summary>
        /// Synchronizes the cache of friends with the data provided by the 
        /// Skype API.
        /// </summary>
        private void UpdateFriends()
        {
            this.friends.Clear();

            var friends = API.Friends;
            
            for(int i = 1; i <= friends.Count; ++i)
            {
                var friend = friends[i];
                this.friends.Add(friend.Handle, friend);
            }

        }

        /// <summary>
        /// Sends a message to the specified skype user using a specified
        /// outgoing channel.
        /// </summary>
        /// <param name="user">The username to send a message to.</param>
        /// <param name="connectionId">The outgoing channel (i.e. incoming channel at remote client)</param>
        /// <param name="o">The message to send. Must be implement Serializable.</param>
        internal void Send(string user, int connectionId, object o)
        {
            var stream = streams[user];

            Exception occurredException = null;
            
            synchronizationContext.Send(delegate
            {
                lock (streams)
                {
                    byte[] prefix = BitConverter.GetBytes(connectionId);
                    byte[] buffer = NetUtils.Serialize(o);

                    byte[] both = new Byte[prefix.Length + buffer.Length];
                    prefix.CopyTo(both, 0);
                    buffer.CopyTo(both, prefix.Length);

                    string base64buffer = Convert.ToBase64String(both);

                    try
                    {
                        stream.Write(base64buffer);
                    }
                    catch(Exception e)
                    {
                        Logger.ErrorFormat("Could not send on Skype Ap2Ap stream: {0}", e);
                        
                        // As skype does not notify us when a stream doesn't work
                        // anymore, we detect here that a stream is not working anymore.
                        foreach(SkypeConnection connection in connections[user].Values)
                        {
                            connection.OnConnectionClosed(new EventArgs());
                        }
                        
                        this.streams.Remove(user);
                        this.connections.Remove(user);
                        this.connectionsForUser.Remove(user);
                        
                        occurredException = new Exception("Could not send Ap2Ap message", e);
                    }
                }
            }, null);
            
            if(occurredException != null)
            {
                throw occurredException;
            }
        }

        private void OnHostSession(HostSessionEventArgs e)
        {
            if (HostSession != null)
            {
                HostSession(this, e);
            }
        }

        private void OnJoinSession(JoinSessionEventArgs e)
        {
            if (JoinSession != null)
            {
                JoinSession(this, e);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Participate not implemented for SkypeProtocol.
        /// </remarks>
        public void Participate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the Skype API ApplicationStreams event
        /// </summary>
        private void ApplicationStreams(SKYPE4COMLib.Application pApp, ApplicationStreamCollection pStreams)
        {
            if (pApp.Name != Application.Name)
                return;

            foreach(ApplicationStream stream in pStreams)
            {
                Console.WriteLine(stream.PartnerHandle + "::" + stream.Handle);
                if (streams.ContainsKey(stream.PartnerHandle))
                {
                    Logger.Info(string.Format("New connection to {0} replaces old one.", pStreams[1].PartnerHandle));
                }
                else
                {
                    connections[stream.PartnerHandle] = new Dictionary<int, SkypeConnection>();
                }
   
                streams[stream.PartnerHandle] = stream;
            }
            Console.WriteLine();
            Console.WriteLine("--");
        }

        /// <summary>
        /// Handles the Skype API ApplicationConnecting event.
        /// </summary>
        private void ApplicationConnecting(SKYPE4COMLib.Application pApp, UserCollection pUsers)
        {
            // TODO: are states correct?
            
            Logger.Info("Connecting: " + pApp.Name + ":: ");
            for(int i = 1; i <= pUsers.Count; ++i)
            {
                Logger.Info(pUsers[i].Handle + " ");
            }


            if (pUsers.Count == 1)
            {
                Logger.Info("Connecting...");
            }

            if (pUsers.Count == 0)
            {
                Logger.Info("Waiting for accept...");
            }
        }

        /// <summary>
        /// Handles the Skype API ApplicationReceiving event.
        /// </summary>
        private void ApplicationReceiving(SKYPE4COMLib.Application pApp, ApplicationStreamCollection pStreams)
        {
            // TODO: move GUI code out of here
            
            if (pStreams.Count == 0)
                return;
            if (pStreams[1].DataLength == 0)
                return;
            string text = pStreams[1].Read();

            // Decode message
            try
            {
                byte[] both = Convert.FromBase64String(text);
                int connectionId = BitConverter.ToInt32(both, 0);

                object message = NetUtils.Deserialize(both, 4, both.Length-4);

                string partner = pStreams[1].PartnerHandle;

                if (connectionId == ApplicationConnectionId)
                {
                    // Received a sharing invitation:
                    // the user might now either accept or reject the invitation
                    // and we'll have to notify the host about his choice.
                    if (message is SharingInvitationMessage)
                    {
                        SharingInvitationMessage sharingInvitation = (SharingInvitationMessage)message;
                        text = string.Format("You received an invitation from {0} via Skype to share document {1}. Would you like to accept?", partner, sharingInvitation.DocumentName);
                        var result = MessageBox.Show(text, "Invitation", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            SkypeConnection connection = Connect(partner);
                            connection.OutgoingChannel = sharingInvitation.Channel;

                            OnJoinSession(new JoinSessionEventArgs(connection));

                            Send(partner, 0, new AcceptSharingInvitationMessage(sharingInvitation.InvitationId, connection.IncomingChannel));
                        }
                        else
                        {
                            Send(partner, 0, new DeclineSharingInvitationMessage(sharingInvitation.InvitationId));
                        }
                    }
                    // Another user has accepted a sharing invitation
                    else if (message is AcceptSharingInvitationMessage)
                    {
                        AcceptSharingInvitationMessage accept = (AcceptSharingInvitationMessage)message;
                        if (!invitations.ContainsKey(accept.InvitationId))
                        {
                            Logger.Error(string.Format("Received {0} but no such invitation was sent.", message));
                            return;
                        }

                        SharingInvitationMessage sharingInvitation = invitations[accept.InvitationId];
                        if (sharingInvitation.InvitedUser != partner)
                        {
                            Logger.Error(string.Format("{0} accepted {1} intended for {2}", partner, sharingInvitation, sharingInvitation.InvitedUser));
                            return;
                        }

                        SkypeConnection connection = invitationChannels[accept.InvitationId];
                        ISynchronizationSession session = invitationSessions[accept.InvitationId];

                        invitations.Remove(accept.InvitationId);
                        invitationChannels.Remove(accept.InvitationId);
                        invitationSessions.Remove(accept.InvitationId);

                        // We have to use a multichannel connection
                        connection.OutgoingChannel = accept.Channel;
                        OnHostSession(new HostSessionEventArgs(session, connection));

                        MessageBox.Show("Invitation was accepted");
                    }

                    // Another user has rejected a sharing invitation
                    else if (message is DeclineSharingInvitationMessage)
                    {
                        DeclineSharingInvitationMessage decline = (DeclineSharingInvitationMessage)message;
                        if (!invitations.ContainsKey(decline.InvitationId))
                        {
                            Logger.Error(string.Format("Received {0} but no such invitation was sent.", message));
                            return;
                        }

                        SharingInvitationMessage sharingInvitation = invitations[decline.InvitationId];
                        if (sharingInvitation.InvitedUser != partner)
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
                    connections[pStreams[1].PartnerHandle][connectionId].ReceiveMessage(new ReceivedEventArgs(message));
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Receive error: {0}", e);
                Logger.Error("Received ill-formed skype datagram", e);
            }
        }
        
        /// <inheritdoc/>
        public event EventHandler<HostSessionEventArgs> HostSession;
        
        /// <inheritdoc/>        
        public event EventHandler<JoinSessionEventArgs> JoinSession;        
    }
}