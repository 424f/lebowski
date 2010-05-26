
namespace Lebowski.Net.Tcp
{
    using System;
    using Lebowski.Synchronization;
    using Lebowski.Synchronization.DifferentialSynchronization;
    
    public class TcpProtocol : ICommunicationProtocol
    {
        public void Share(ISynchronizationSession session)
        {
            TcpShareForm form = new TcpShareForm();
            form.Submit += delegate
            {
                form.Enabled = false;
                session.State = SessionStates.AwaitingConnection;
                form.Invoke((Action) delegate
                {
                    form.Dispose();
                });
                TcpServerConnection connection = new TcpServerConnection(form.Port);
                
                connection.ClientConnected += delegate
                {
                    OnHostSession(new HostSessionEventArgs(session, connection));
                };
            };
            form.ShowDialog();
        }

        public string Name
        {
            get { return "TCP"; }
        }
        
        
        public bool CanShare
        {
            get { return true; }
        }

        public bool CanParticipate
        {
            get { return true; }
        }

        protected virtual void OnHostSession(HostSessionEventArgs e)
        {
            if (HostSession != null)
            {
                HostSession(this, e);
            }
        }

        protected virtual void OnJoinSession(JoinSessionEventArgs e)
        {
            if (JoinSession != null)
            {
                JoinSession(this, e);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Calling this will display a form to the user to enter connection
        /// information.
        /// </remarks>
        public void Participate()
        {
            TcpParticipateForm form = new TcpParticipateForm();

            form.Submit += delegate
            {
                try
                {
                    TcpClientConnection connection = new TcpClientConnection(form.Address, form.Port);
                    OnJoinSession(new JoinSessionEventArgs(connection));
                }
                catch(ConnectionFailedException)
                {
                    System.Windows.Forms.MessageBox.Show("Could not connect to " + form.Address + ":" + form.Port);
                }
                catch(Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred:\n" + e.ToString(), "Error");
                }
                finally
                {
                    form.Close();
                }
            };

            form.ShowDialog();
        }

        /// <inheritdoc/>
        /// <remarks>The TCP protocol does not have any external dependencies,
        /// so it is always enabled.</remarks>
        public bool Enabled
        {
            get { return true; }
        }
        
        /// <inheritdoc/>
        public event EventHandler<HostSessionEventArgs> HostSession;
        
        /// <inheritdoc/>
        public event EventHandler<JoinSessionEventArgs> JoinSession;

    }
}