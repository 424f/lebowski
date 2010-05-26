namespace Lebowski.Synchronization.DifferentialSynchronization
{
    using System;
    /// <summary>
    /// Indicates whether a site currently holds the token.
    /// 
    /// In the differential synchronization between two sites, only one site
    /// ever holds a token, which allows it to distribute its changes back
    /// to the other site. If it notices local changes but does not hold the token,
    /// it has to first request it from the other site.
    /// </summary>
    public enum TokenState
    {
        /// <summary>
        /// Indicates that the site currently does not hold the token
        /// and first has to request it, before it can propagate its changes
        /// when needed.
        /// </summary>
        WaitingForToken,
        
        /// <summary>
        /// Indicates that the site currently holds the token and can
        /// distribute its changes at any time.
        /// </summary>
        HavingToken
    };
}