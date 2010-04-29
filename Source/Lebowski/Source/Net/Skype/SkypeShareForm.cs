
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lebowski.Net.Skype
{
	public partial class SkypeShareForm : Form
	{
		public event EventHandler Submit;
		
		private SkypeProtocol protocol;
		
		public string SelectedUser
		{
			get
			{
				return (string)dataGridView.SelectedRows[0].Cells[1].Value;
			}
		}
		
		public SkypeShareForm(SkypeProtocol protocol)
		{
			InitializeComponent();			
			
			this.protocol = protocol;
			
			foreach(string username in protocol.friends)
			{
				dataGridView.Rows.Add(new object[]{ null, username });
			}
		}
		
		void SkypeShareFormLoad(object sender, EventArgs e)
		{
			
		}
		
		void ShareButtonClick(object sender, EventArgs e)
		{
			OnSubmit(new EventArgs());
		}
		
		protected virtual void OnSubmit(EventArgs e)
		{
			if (Submit != null) {
				Submit(this, e);
			}
		}
		
	}
}
