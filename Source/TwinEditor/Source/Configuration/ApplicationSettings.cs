namespace TwinEditor.Configuration
{
    using System;
    using System.Configuration; 
    using System.Collections.Generic;
    
    /// <summary>
    /// <param>Application- and Userlevel Settings.</param>
    /// <param>For proper usage consult http://msdn.microsoft.com/de-de/library/0zszyc6e%28v=VS.80%29.aspx.</param>
    /// </summary>
    public class ApplicationSettings : ApplicationSettingsBase
    {
        /// <value>
        /// Default instance of the application settings.
        /// </value>
        private static ApplicationSettings defaultInstance = (ApplicationSettings)ApplicationSettingsBase.Synchronized(new ApplicationSettings());
        
        /// <summary>
        /// Returns the default instance as a singleton.
        /// </summary>
        public static ApplicationSettings Default
        {
            get { return defaultInstance; }
        }
        
        /// <summary>
        /// Initializes a new instance of ApplicationSettings.
        /// </summary>
        public ApplicationSettings()
        {
        }
        
        #region Settings
        /// <summary>
        /// Username of current user.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }
        
        /// <summary>
        /// Synchronization strategy to be used when sharing a session.
        /// </summary>
        //[ApplicationScopedSettingAttribute]
        [UserScopedSettingAttribute]
        [DefaultSettingValue("Lebowski.DifferentialSynchronization.DifferentialSynchronizationStrategy")]
        public string SynchronizationStrategy
        {
            get { return (string)this["SynchronizationStrategy"]; }
            set { this["SynchronizationStrategy"] = value; }
        }
        
        /// <summary>
        /// The recent files stored in a list of filenames.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public List<string> RecentFileList
        {
            get { return (List<string>)this["RecentFileList"]; }
            set { this["RecentFileList"] = value; }
        }
        
        /// <summary>
        /// The recent files list size.
        /// </summary>
        [UserScopedSettingAttribute]
        [DefaultSettingValue("5")]
        public string RecentFileListSize
        {
        	get { return (string)this["RecentFileListSize"]; }
            set { this["RecentFileListSize"] = value; }
        }
        #endregion
    }
}
