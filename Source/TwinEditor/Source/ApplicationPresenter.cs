using System;
using System.IO;
using System.Linq;
using Lebowski;
using Lebowski.Net;
using Lebowski.Synchronization.DifferentialSynchronization;
using TwinEditor.UI;
using TwinEditor.FileTypes;

namespace TwinEditor
{
	public class ApplicationPresenter
	{
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
					MultichannelConnection mcc = new MultichannelConnection(e.Connection);
					var syncConnection = mcc.CreateChannel();
					var sync = new DifferentialSynchronizationStrategy(0, e.Session.Context, syncConnection);
					sync.EstablishSession();

					e.Session.StartSession(sync, mcc.CreateChannel());
					
				};
				
				protocol.JoinSession += delegate(object sender, JoinSessionEventArgs e)
				{ 
					// TODO: choose correct file type
					ISession tab = view.CreateNewSession(fileTypes[0]);
					
					MultichannelConnection mcc = new MultichannelConnection(e.Connection);
					var syncConnection = mcc.CreateChannel();
					var sync = new DifferentialSynchronizationStrategy(1, tab.Context, syncConnection);
					tab.StartSession(sync, mcc.CreateChannel());
					
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
    			ISession tab = view.CreateNewSession(e.FileType);
    			tab.FileName = e.FileName;
    			
    			// Put content into editor
    			// case when user cancelled dialog not handled yet
    			string content = File.ReadAllText(e.FileName);
    			tab.Context.Data = content;
            };
		}
	}
}
