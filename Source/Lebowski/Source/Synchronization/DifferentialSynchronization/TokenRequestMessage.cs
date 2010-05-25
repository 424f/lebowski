using System;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
    /// <summary>
    /// This message is sent when a client has to propagate local changes, 
    /// but does not currently hold the token.
    /// </summary>
    [Serializable]
    class TokenRequestMessage
    {
        public override string ToString()
        {
            return "TokenRequestMessage()";
        }
    }
}
