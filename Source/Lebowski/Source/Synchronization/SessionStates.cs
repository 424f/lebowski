
namespace Lebowski
{
    using System;
    /// <summary>
    /// Describes the state of a collaborative editing session
    /// </summary>
    public enum SessionStates
    {
        /// <summary>
        /// The session is currently not being shared
        /// </summary>
        Disconnected,

        /// <summary>
        /// The session is currently being shared and is waiting for a participant
        /// </summary>
        AwaitingConnection,

        /// <summary>
        /// We are attempting to connect to a remote session
        /// </summary>
        Connecting,

        /// <summary>
        /// A shared session has been established
        /// </summary>
        Connected
    }
}