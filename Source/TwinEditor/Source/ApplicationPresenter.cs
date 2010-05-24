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
	public class ApplicationPresenter
	{
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationPresenter));	    
	    
		public ApplicationPresenter(IApplicationView view)
		{
            // Provide view with file types
			var fileTypes = ExtensionUtil.FindTypesImplementing(typeof(IFileType))
				.Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
				.Cast<IFileType>()
				.ToArray();
            view.FileTypes = fileTypes;
            
            // Load available protocols and subscribe to events
            var protocols = ExtensionUtil.FindTypesImplementing(typeof(ICommunicationProtocol))
    				.Select((t) => t.GetConstructor(new Type[]{}).Invoke(new object[]{}))
    				.Cast<ICommunicationProtocol>()
    				.ToArray();            
            view.CommunicationProtocols = protocols;
            
            foreach(ICommunicationProtocol protocol in protocols)
            {
				// Register to communication protocol events
				protocol.HostSession += delegate(object sender, HostSessionEventArgs e)
				{ 
				    SessionContext context = (SessionContext)e.Session;
				    context.EstablishSharedSession(0, e.Connection);					
				};
				
				protocol.JoinSession += delegate(object sender, JoinSessionEventArgs e)
				{ 
				    ISessionView sessionView = view.CreateNewSession(fileTypes[0]);
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
