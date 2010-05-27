using System.Collections.Generic;
namespace TwinEditor
{
    using System;
    using System.IO;
    using System.Linq;

    using Lebowski;
    using Lebowski.Net;
    using Lebowski.Synchronization.DifferentialSynchronization;

    using log4net;

    using TwinEditor.FileTypes;
    using TwinEditor.UI;
    using TwinEditor.Sharing;

    // TODO: this should be better separated into Model / Controller, as it 
    // currently basically contains both.
    
    /// <summary>
    /// Contains behavior of the application. It is used with a IApplicationView that
    /// subscribes to its events and displays data to the user.
    /// </summary>
    public class ApplicationContext
    {
        /// <summary>Provides loggin facilities.</summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationContext));

        /// <value>
        /// The file types that are supported currently
        /// </value>
        public IFileType[] FileTypes { get; private set; }

        /// <value>
        /// The communication protocols currently supported
        /// </value>
        public ICommunicationProtocol[] CommunicationProtocols { get; private set; }

        private RecentFilesList recentFilesList;
        private List<SessionContext> currentSessions = new List<SessionContext>();

        /// <summary>
        /// Initializes a new instance of the ApplicationContext class.
        /// </summary>
        /// <param name="view">the view that we synchronize with</param>        
        public ApplicationContext(IApplicationView view)
        {
            // Provide view with file types
            FileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType))
                .Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
                .Cast<IFileType>()
                .ToArray();
            view.FileTypes = FileTypes;

            // Load available protocols and subscribe to events
            CommunicationProtocols = ExtensionUtil.FindTypesImplementing(typeof(ICommunicationProtocol))
                    .Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
                    .Cast<ICommunicationProtocol>()
                    .ToArray();
            view.CommunicationProtocols = CommunicationProtocols;

            foreach (ICommunicationProtocol protocol in CommunicationProtocols)
            {
                // Register to communication protocol events
                protocol.HostSession += delegate(object sender, HostSessionEventArgs e)
                {
                    SessionContext context = (SessionContext)e.Session;
                    context.EstablishSharedSession(0, e.Connection);
                };

                protocol.JoinSession += delegate(object sender, JoinSessionEventArgs e)
                {
                    // TODO: choose correct file type
                    ISessionView sessionView = view.CreateNewSession(FileTypes[0]);
                    currentSessions.Add(sessionView.SessionContext);
                    sessionView.SessionContext.EstablishSharedSession(1, e.Connection);
                };
            }

            // Protocol: sharing
            view.ShareSession += delegate(object sender, ShareSessionEventArgs args)
            {
                try
                {
                    args.Protocol.Share(args.Session);
                }
                catch(Exception e)
                {
                    args.Session.Reset();
                    Logger.WarnFormat("Could not share session with {0}:\n{1}", args.Protocol.Name, e);
                    view.DisplayError("The session could not be shared.", e);
                }
            };
            
            // Protocol: participate
            view.Participate += delegate(object sender, ParticipateEventArgs args)
            {
                try
                {
                    args.Protocol.Participate();
                }
                catch(Exception e)
                {
                    Logger.WarnFormat("Could not participate with protocol {0}:\n{1}", args.Protocol.Name, e);
                    view.DisplayError("Participating in a session failed.", e);
                }
            };            

            // File handling
            view.NewFile += delegate(object sender, NewFileEventArgs e)
            {
                ISessionView sessionView = view.CreateNewSession(e.FileType);
                currentSessions.Add(sessionView.SessionContext);
            };
            
            view.OpenFile += delegate(object sender, OpenFileEventArgs e)
            {
                SessionContext sessionContext = currentSessions.Find((SessionContext s) => s.FileName == e.FileName);
                
                if (sessionContext != null)
            	{
            		Logger.Info(e.FileName + " already open");
            		// TODO: activate tab
            	}
            	else
            	{
                    // Determines file's type
                    IFileType type = null;
                    foreach (IFileType fileType in FileTypes)
                    {
                    	if (fileType.FileNameMatches(e.FileName))
        					type = fileType;
                    }
        
                    // No appropriate file type has been found
                    // Should never happen as we already filter file types in the openFileDialog
                    if (type == null)
                    {
                        view.DisplayError(string.Format((TranslationUtil.GetString(ApplicationUtil.LanguageResources, "_MessageBoxUnsupportedFileType") + " '{0}'"), e.FileName), new Exception());
                        return;
                    }            	    
            	    
            		// Create a new tab for the file
		            ISessionView tab = view.CreateNewSession(type);
		            currentSessions.Add(tab.SessionContext);
		            
		            // Put content into editor
		            string content = File.ReadAllText(e.FileName);
		            tab.SessionContext.Context.Data = content;
		            tab.OnDisk = true;
		            tab.FileModified = false;
		            tab.SessionContext.FileName = e.FileName;
		            Logger.Info("Openend " + e.FileName);
		            // update recent files
		            recentFilesList.Add(e.FileName);
            	}
            };
            
            view.CloseFile += delegate(object sender, CloseFileEventArgs e)
            {
            	Logger.Info("Closed " + e.Session.FileName);
            	currentSessions.Remove(e.Session);
            };
            
            view.SaveFile += delegate(object sender, SaveFileEventArgs e)
            {
                try
                {
                    var tabControl = e.Session;
                    e.Session.FileName = e.FileName;
                    File.WriteAllText(tabControl.FileName, e.Session.Context.Data);
                    // update recent files
                    recentFilesList.Add(e.FileName);
                    Logger.Info(string.Format("{0} has been saved successfully", tabControl.FileName));
                }
                catch (Exception exc)
                {
                    Logger.Error(string.Format("{0} could not be saved due to {1}", e.FileName, exc.Message));
                }
            };
            
            // Application closing
            view.ApplicationClosing += delegate {
                foreach(SessionContext sessionContext in currentSessions)
                {
                    sessionContext.Close();
                }
                System.Windows.Forms.Application.Exit();
                Environment.Exit(0);
            };
            
            // Restore or initialize recentFilesList
            var appSettings = Configuration.ApplicationSettings.Default;

           	List<string> recentFiles = appSettings.RecentFileList;
            if (recentFiles != null)
            {
            	recentFilesList = new RecentFilesList(int.Parse(appSettings.RecentFileListSize), recentFiles);
            	Logger.Info("Restored recent files list");
            }
            else
            {
            	recentFilesList = new RecentFilesList((int.Parse(appSettings.RecentFileListSize)));
            	Logger.Info("No recent files found");
            }
            view.UpdateRecentFiles(recentFiles);
            recentFilesList.RecentFilesChanged += delegate(object sender, RecentFilesChangedEventArgs e) 
            {
            	view.UpdateRecentFiles(e.RecentFiles);
            };
            
        }
    }
}
