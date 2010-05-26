namespace Lebowski.Net.Multichannel
{
    using System;    
    
    class TunneledConnection : AbstractConnection
    {
        private MultichannelConnection tunnel;
    
        public TunneledConnection(MultichannelConnection tunnel)
        {
            this.tunnel = tunnel;
        }
    
        public override void Send(object o)
        {
            tunnel.Send(this, o);
        }
    
        public override void Close()
        {
            this.tunnel.Close();
        }
        
        internal new void OnReceived(ReceivedEventArgs e)
        {
            base.OnReceived(e);
        }
    }
}
