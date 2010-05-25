namespace Lebowski.Net
{
    using System;
    
    /// <summary>
    /// Provides functionality to share a session or participate in an existing
    /// session and describes the exact capabilities that the protocol supports.
    /// </summary>
    public interface ICommunicationProtocol
    {
        /// <summary>
        /// Protocol name that should be displayed to the user
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Shares an existing single-user session using this protocol
        /// </summary>
        /// <param name="session"></param>
        void Share(ISynchronizationSession session);

        /// <summary>
        /// Participates in an existing session, usually by first displaying
        /// configuration options to the user.
        /// </summary>
        void Participate();

        /// <summary>
        /// Does this protocol allow the user to share a document? This should
        /// generally be true, but might be false for protocols using
        /// a central synchronization server.
        /// </summary>
        bool CanShare { get; }

        /// <summary>
        /// Does this protocol provide functionality to actively connect
        /// to an existing session? Some protocols might set this to false
        /// as they establish connections based on invitations.
        /// </summary>
        bool CanParticipate { get; }

        /// <summary>
        /// Occurs when a session has been hosted using this protocol
        /// </summary>
        event EventHandler<HostSessionEventArgs> HostSession;

        /// <summary>
        /// Occurs when a session is joined using this protocol
        /// </summary>
        event EventHandler<JoinSessionEventArgs> JoinSession;

        /// <summary>
        /// Indicates whether this protocol should be displayed to the user. This might be used
        /// to disable unstable protocols in release mode or to hide a protocol when the needed
        /// requirements are not met by the system.
        /// </summary>
        bool Enabled { get; }
    }
}