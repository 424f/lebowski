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
                    sessionView.SessionContext.EstablishSharedSession(1, e.Connection);
                };                    
            }
            
            // Protocol: sharing
            view.ShareSession += delegate(object sender, ShareSessionEventArgs e)
            {
                e.Protocol.Share(e.Session);
            };
            
            // File handling
            view.Open += delegate(object sender, OpenEventArgs e)
            {
                // Create a new tab for the file
                ISessionView tab = view.CreateNewSession(e.FileType);
                tab.FileName = e.FileName;
                
                // Put content into editor
                // case when user cancelled dialog not handled yet
                string content = File.ReadAllText(e.FileName);
                tab.SessionContext.Context.Data = content;
            };
            
            view.Save += delegate(object sender, SaveEventArgs e)
            {
                try
                {
                    var tabControl = e.Session;
                    e.Session.FileName = e.FileName;
                    File.WriteAllText(tabControl.FileName, tabControl.SessionContext.Context.Data);
                    Logger.Info(string.Format("{0} has been saved successfully", tabControl.FileName));
                }
                catch (Exception exc)
                {
                    Logger.Error(string.Format("{0} could not be saved due to {1}", e.FileName, exc.Message));
                }
            };
        }
    }
}
