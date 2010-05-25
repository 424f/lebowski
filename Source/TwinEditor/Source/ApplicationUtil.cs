
namespace TwinEditor
{
    using System;
    using System.Resources;
    using System.Reflection;
    public static class ApplicationUtil
    {
        private static bool isInitialized = false;
        
        /// <summary>
        /// The language resources for the current language
        /// </summary>
        public static ResourceManager LanguageResources { get; private set; }
        
        /// <summary>
        /// Main application resources, including images and single-language text
        /// </summary>
        public static ResourceManager Resources { get; private set; }
        
        public static void Initialize()
        {
            if (isInitialized)
                return;
            
            LanguageResources = new ResourceManager("TwinEditor.LanguageResources", Assembly.GetExecutingAssembly());
            Resources = new ResourceManager("TwinEditor.Resources", Assembly.GetExecutingAssembly());
            
            isInitialized = true;
        }
        
        
    }
}
