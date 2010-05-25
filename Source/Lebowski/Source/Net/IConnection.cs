namespace Lebowski.Net
{
    using System;
    
    /// <summary>
    /// Provides functionality for bi-directional communication with another host.
    /// Outgoing communication is performed with <see cref="Lebowski.Net.IConnection.Send">Send</see>,
    /// whereas to react to incoming messages, the client has to subscribe to the
    /// <see cref="Lebowski.Net.IConnection.Received">Received</see> event.
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Serializes an object and sends it as a packet to the endpoint.
        /// </summary>
        /// <param name="o">The serializable object that is to be sent.</param>
        void Send(object o);
        
        /// <summary>
        /// Closes this connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Occurs when a packet is received from the endpoint.
        /// </summary>
        event EventHandler<ReceivedEventArgs> Received;

        /// <summary>
        /// Can be used to associate application-specific data with this connection.
        /// </summary>
        object Tag { get; set; }
        
        /// <summary>
        /// Occurs when this connection is closed.
        /// </summary>
        event EventHandler<EventArgs> ConnectionClosed;
    }
}