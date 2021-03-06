﻿namespace TwinEditor.Sharing
{
    using System;
    using Lebowski.Synchronization.DifferentialSynchronization;
    using TwinEditor.Messaging;
    
    /// <summary>
    /// The InitializationState makes sure that the client receives
    /// the initial context present on the server, so they are initially
    /// synchronized.
    /// </summary>
    internal class InitializationState : SessionState
    {
        /// <summary>
        /// Initializes a new instance of the InitializationState class.
        /// </summary>
        /// <param name="session"></param>
        internal InitializationState(SessionContext session) : base(session)
        {
        }
        
        /// <inheritdoc />
        internal override void Register()
        {
            // Server sends his state to client and then changes to SynchronizationState
            if(session.SiteId == 0)
            {
                session.SynchronizationStrategy = new DifferentialSynchronizationStrategy(session.SiteId, session.Context, session.SynchronizationConnection);
                session.ApplicationConnection.Send(new InitialContextMessage(session.Context.Data));
                session.ActivateState(new SynchronizationState(session));
            }
            session.ApplicationConnection.Received += ApplicationConnectionReceived;
        }

        /// <inheritdoc />        
        internal override void Unregister()
        {
            session.ApplicationConnection.Received -= ApplicationConnectionReceived;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Handles the following message types: <see cref="InitialContextMessage">InitialContextMessage</see>.
        /// </remarks>        
        protected override void ApplicationConnectionReceived(object sender, Lebowski.Net.ReceivedEventArgs e)
        {
            if(e.Message is InitialContextMessage)
            {
            	session.Context.Invoke((Action)
					delegate
					{
		                session.Context.Data = ((InitialContextMessage)e.Message).Text;
		                session.SynchronizationStrategy = new DifferentialSynchronizationStrategy(session.SiteId, session.Context, session.SynchronizationConnection);
		                session.ActivateState(new SynchronizationState(session));
					}
				);
            }
            else
            {
                base.ApplicationConnectionReceived(sender, e);
            }
        }
    }
}
