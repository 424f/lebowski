namespace TwinEditor.UI
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    
    /// <summary>
    /// A UI control that provides the same functionality as TabControl,
    /// but also provides a way for the user to close certain tabs.
    /// </summary>
    public class ClosableTabControl : TabControl
    {
        /// <summary>
        /// Occurs when the close context menu of a closeable tab has been clicked.
        /// </summary>
        public event EventHandler<TabClosedEventArgs> TabClosed;

        private ContextMenuStrip tabContextMenuStrip;
        private int ClickedTabIndex = -1;

        /// <summary>
        /// Default constructor
        /// Initializes a new instance of CloseableTabControl
        /// All tabs are closeable
        /// </summary>
        public ClosableTabControl() : this(0)
        {

        }

        /// <summary>
        /// Constructor
        /// Initializes a new instance of CloseableTabControl whose first
        /// closable tab has the specified index.
        /// </summary>
        /// <param name="firstClosableTabIndex">See <see cref="FirstClosableTabIndex">FirstClosableTabIndex</see>.</param>
        public ClosableTabControl(int firstClosableTabIndex)
        {
            FirstClosableTabIndex = firstClosableTabIndex;
        }

        /// <summary>
        /// Specifies which is the first tab that is closable.
        /// </summary>
        public int FirstClosableTabIndex { get; set; }         
        
        /// <summary>
        /// Eventhadler delegate for handling the event when a click on the close context menu of a closeable tab is detected.
        /// </summary>
        /// <param name="sender"><see cref="object"></see></param>
        /// <param name="e"><see cref="System.EventArgs"></see></param>
        private void CloseClicked(object sender, EventArgs e)
        {
            // Source tab (0) should not be closed
            if (this.ClickedTabIndex >= 0)
            {
                OnTabClosed(new TabClosedEventArgs(this.ClickedTabIndex));
            }
        }

        /// <summary>
        /// Overwrites the default OnMouseClick.
        /// </summary>
        /// <remarks>
        /// Compares the click position with the tab header position.
        /// </remarks>
        /// <param name="e"><see cref="System.Windows.Forms.MouseEventArgs"></see></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = e.Location;
                // iterate over all tabs execpt of the first tab
                for (int i = FirstClosableTabIndex; i < this.TabCount; i++)
                {
                    // gets the rectangle of the curent tab (header)
                    Rectangle rectangle = GetTabRect(i);

                    // check if clicked position is contained in the rectangle, if yes close the tab
                    if (rectangle.Contains(p))
                    {
                        this.ClickedTabIndex = i;
                        // show contextMenuStrip
                        tabContextMenuStrip = new ContextMenuStrip();
                        // add close menu item and corredsponding event handler
                        tabContextMenuStrip.Items.Add(TranslationUtil.GetString(ApplicationUtil.LanguageResources, "Close"));
                        tabContextMenuStrip.Items[0].Click += new EventHandler(CloseClicked);
                        tabContextMenuStrip.Show(this.PointToScreen(p));
                    }
                }
            }
        }

        /// <summary>
        /// Fires <see cref="TabClosedEventArgs"></see>
        /// </summary>
        /// <param name="e"><see cref="TabClosedEventArgs"></see></param>
        protected void OnTabClosed(TabClosedEventArgs e)
        {
            if (TabClosed != null)
            {
                TabClosed(this, e);
            }
        }
    }

    /// <summary>
    /// Provides data for the <see cref="ClosableTabControl.TabClosed" /> event.
    /// </summary>
    public sealed class TabClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the TabClosedEventArgs class.
        /// </summary>
        /// <param name="index"><see cref="TabIndex" /></param>
        public TabClosedEventArgs(int index)
        {
            TabIndex = index;
        }
        
        /// <summary>
        /// The index of the tab that was closed.
        /// </summary>
        public int TabIndex { get; private set; }        
    }
}


