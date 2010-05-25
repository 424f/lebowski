namespace TwinEditor.Configuration
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Collections.Generic;

    public class ApplicationSettings : ApplicationSettingsBase
    {
        private static ApplicationSettings defaultInstance = (ApplicationSettings)ApplicationSettingsBase.Synchronized(new ApplicationSettings());

        public static ApplicationSettings Default
        {
            get { return defaultInstance; }
        }

        public ApplicationSettings()
        {
        }

        #region Settings
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }

        //[ApplicationScopedSettingAttribute]
        [UserScopedSettingAttribute]
        [DefaultSettingValue("Lebowski.DifferentialSynchronization.DifferentialSynchronizationStrategy")]
        public string SynchronizationStrategy
        {
            get { return (string)this["SynchronizationStrategy"]; }
            set { this["SynchronizationStrategy"] = value; }
        }

        //[ApplicationScopedSettingAttribute]
        [UserScopedSettingAttribute]
        [DefaultSettingValue(null)]
        public List<string> RecentFileList
        {
            get { return (List<string>)this["RecentFileList"]; }
            set { this["RecentFileList"] = value; }
        }
        #endregion
    }
}