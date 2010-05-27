namespace TwinEditor
{
    using System;
    using System.Windows.Forms;
    using System.Resources;
    using System.Collections.Generic;
    using log4net;
    
    /// <summary>
    /// Provides helper methods that allows for retrieval of language-specific
    /// resources.
    /// </summary>
    public static class TranslationUtil
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TranslationUtil));

        /// <summary>
        /// Retrieves a resource string, issueing a warning if the resource
        /// identifier does not exist.
        /// </summary>
        /// <param name="resources">The resource manager that provides the translations.</param>
        /// <param name="id">The resource's identifier.</param>
        /// <returns></returns>
        public static string GetString(ResourceManager resources, string id)
        {
            string result = resources.GetString(id);
            if (result == null)
            {
                result = "{" + id + "}";
                Logger.Warn("No translation found for " + id + ".");
            }
            return result;
        }

        /// <summary>
        /// Translates all items found in a menuStrip. Initially, <see cref="ToolStripMenuItem">ToolStripMenuItem</see>s
        /// in this control must contain the resource identifier as the Text.
        /// property value.
        /// </summary>
        /// <param name="menuStrip">The Menu strip to be translated.</param>
        /// <param name="resources">The resource manager that provides the translations.</param>
        public static void TranslateMenuStrip(MenuStrip menuStrip, ResourceManager resources)
        {
            Queue<ToolStripMenuItem> menuItems = new Queue<ToolStripMenuItem>();
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                menuItems.Enqueue(item);
            }

            while (menuItems.Count != 0)
            {
                ToolStripMenuItem item = menuItems.Dequeue();
                item.Text = GetString(resources, item.Text);
                foreach (object child in item.DropDown.Items)
                {
                    if (child is ToolStripMenuItem)
                    {
                        menuItems.Enqueue((ToolStripMenuItem)child);
                    }
                }
            }
        }
    }
}