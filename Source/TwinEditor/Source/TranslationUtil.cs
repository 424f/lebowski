
namespace TwinEditor
{
    using System;
    using System.Windows.Forms;
    using System.Resources;
    using System.Collections.Generic;
    using log4net;
    public static class TranslationUtil
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TranslationUtil));
        
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
