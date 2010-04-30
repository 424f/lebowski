using System;
using Lebowski.Synchronization.DifferentialSynchronization;

namespace Lebowski.Net.Tcp
{
	public class TcpProtocol : ICommunicationProtocol
	{
		public event EventHandler<HostSessionEventArgs> HostSession;
		public event EventHandler<JoinSessionEventArgs> JoinSession;
		
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
				TcpServerConnection connection = new TcpServerConnection();	
				connection.ClientConnected += delegate
				{
					MultichannelConnection mcc = new MultichannelConnection(connection);
					OnHostSession(new HostSessionEventArgs(session, mcc.CreateChannel(), mcc.CreateChannel()));					
					form.Invoke((Action) delegate
					{
						form.Close();
					});
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
			if (HostSession != null) {
				HostSession(this, e);
			}
		}
		
		protected virtual void OnJoinSession(JoinSessionEventArgs e)
		{
			if (JoinSession != null) {
				JoinSession(this, e);
			}
		}
	
		public void Participate()
		{
			TcpParticipateForm form = new TcpParticipateForm();
			
			form.Submit += delegate
			{
				try
				{
					TcpClientConnection connection = new TcpClientConnection(form.Address);
					MultichannelConnection mcc = new MultichannelConnection(connection);
					OnJoinSession(new JoinSessionEventArgs(mcc.CreateChannel(), mcc.CreateChannel()));
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
