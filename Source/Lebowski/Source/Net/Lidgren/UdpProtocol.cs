
namespace Lebowski.Net.Lidgren
{
    using System;
    public class UdpProtocol : ICommunicationProtocol
    {
        public string Name
        {
            get { return "UDP"; }
        }
        
        public bool CanShare
        {
            get { return true; }
        }
        
        public bool CanParticipate
        {
            get { return true; }
        }
        
        public void Share(ISynchronizationSession session)
        {
            throw new NotImplementedException();
        }
        
        public event EventHandler<HostSessionEventArgs> HostSession;
        public event EventHandler<JoinSessionEventArgs> JoinSession;

        public void Participate()
        {
            throw new NotImplementedException();
        }
        
        public bool Enabled
        {
            get { return false; }
        }
        
    }
}
