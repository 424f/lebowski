using System;
using System.Windows.Forms;
using System.Drawing;

namespace TwinEditor
{
	/// <summary>
	/// Description of CloseabletabControl.
	/// </summary>
	public class CloseableTabControl : TabControl
	{
		public event EventHandler<TabClosedEventArgs> TabClosed;
		
		private ContextMenuStrip tabContextMenuStrip;
		private int ClickedTabIndex = -1;
		public int Shift { get; private set; } // tab index from which on the tabs can be closed
		
		public CloseableTabControl(int shift)
		{
			this.Shift = shift;
  			// create new contextMenuStrip
            tabContextMenuStrip = new ContextMenuStrip();
            // add close menu item and corredsponding event handler
            tabContextMenuStrip.Items.Add(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "Close"));
            tabContextMenuStrip.Items[0].Click += new EventHandler(CloseClicked);
            // TODO: Execution results sharing can be triggered here
        }

        private void CloseClicked(object sender, EventArgs e)
        {
        	// Source tab (0) should not be closed
            if (this.ClickedTabIndex >= 0)
            {
                OnTabClosed(new TabClosedEventArgs(this.ClickedTabIndex));
            }
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = e.Location;
                // iterate over all tabs execpt of the first tab
                for (int i = Shift; i < this.TabCount; i++)
                {
                    // gets the rectangle of the curent tab (header)
                    Rectangle rectangle = GetTabRect(i);

                    // check if clicked position is contained in the rectangle, if yes close the tab
                    if (rectangle.Contains(p))
                    {
                        this.ClickedTabIndex = i;
                        // show contextMenuStrip
                        tabContextMenuStrip.Show(this.PointToScreen(p));
                    }
                }
            }
        }
        
        protected void OnTabClosed(TabClosedEventArgs e)
        {
        	if (TabClosed != null)
        	{
        		TabClosed(this, e);
        	}
        }
	}
	
	public sealed class TabClosedEventArgs : EventArgs
	{
		public int TabIndex { get; private set; }
		public TabClosedEventArgs(int index)
		{
			TabIndex = index;
		}
	}
}


	
