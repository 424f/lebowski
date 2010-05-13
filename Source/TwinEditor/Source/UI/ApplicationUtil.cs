using System;
using System.Resources;
using System.Reflection;

namespace TwinEditor
{
    public static class ApplicationUtil
    {
        private static bool isInitialized = false;
        
        public static ResourceManager LanguageResources { get; private set; }
        
        public static void Initialize()
        {
            if(isInitialized)
            {
                return;
            }
            LanguageResources = new ResourceManager("TwinEditor.LanguageResources", Assembly.GetExecutingAssembly());
        }
    }
}
