namespace TwinEditor.Configuration
{
    using System;
    using System.Configuration; 
    using System.Collections.Generic;
    
    /// <summary>
    /// Provides per-user and per-application settings associated with this 
    /// application.
    /// </summary>
    public class ApplicationSettings : ApplicationSettingsBase
    {
        private static ApplicationSettings defaultInstance = (ApplicationSettings)ApplicationSettingsBase.Synchronized(new ApplicationSettings());
        
        /// <summary>
        /// Gets the default instance of these settings.
        /// </summary>
        /// <remarks>
        /// This instance is statically initialized.
        /// </remarks>
        public static ApplicationSettings Default
        {
            get { return defaultInstance; }
        }
               
        /// <summary>
        /// Gets or sets the configured username on a per-user level.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the name of the ISynchronizationStrategy to be used
        /// by default on a per-user level.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue("Lebowski.DifferentialSynchronization.DifferentialSynchronizationStrategy")]
        public string SynchronizationStrategy
        {
            get { return (string)this["SynchronizationStrategy"]; }
            set { this["SynchronizationStrategy"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the recently opened files on a per-user level.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public List<string> RecentFileList
        {
            get { return (List<string>)this["RecentFileList"]; }
            set { this["RecentFileList"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the length of the recently opened files list on a
        /// per-user level.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue("5")]
        public string RecentFileListSize
        {
        	get { return (string)this["RecentFileListSize"]; }
            set { this["RecentFileListSize"] = value; }
        }
    }
}
