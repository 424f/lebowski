using System;
using System.IO;
using System.Linq;
using Lebowski;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.UI;
using TwinEditor.FileTypes;
using log4net;

namespace TwinEditor
{
	public class ApplicationContext
	{
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationContext));	    
	    
        public IFileType[] FileTypes { get; private set; }
        
        public ICommunicationProtocol[] CommunicationProtocols { get; private set; }
        
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
            
            foreach(ICommunicationProtocol protocol in CommunicationProtocols)
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
