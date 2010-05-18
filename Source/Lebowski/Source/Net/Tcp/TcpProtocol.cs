using System;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace Lebowski.Net.Tcp
{	
	public class TcpProtocol : ICommunicationProtocol
	{
		public event EventHandler<HostSessionEventArgs> HostSession;
		public event EventHandler<JoinSessionEventArgs> JoinSession;
		public event EventHandler<EventArgs> Waiting;
		
		public string Name
		{
			get { return "TCP"; }
		}
		
		public void Share(ISessionContext session)
		{
			TcpShareForm form = new TcpShareForm();
			form.Submit += delegate
			{
				form.Enabled = false;
				form.Invoke((Action) delegate
				{
					form.Close();
				});
				// TODO: Fire waiting event and implement indicator in UI
				TcpServerConnection connection = new TcpServerConnection(form.Port);
				connection.ClientConnected += delegate
				{
					OnHostSession(new HostSessionEventArgs(session, connection));
				};
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
		
		protected virtual void OnWaiting(EventArgs e)
		{
			if (Waiting != null)
			{
				Waiting(this, e);
			}
		}
	
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
				catch(ConnectionFailedException e)
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
		
		public bool Enabled
		{
			get { return true; }
		}				
	}
}
