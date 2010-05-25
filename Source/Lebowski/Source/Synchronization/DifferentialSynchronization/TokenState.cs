
namespace Lebowski.Synchronization.DifferentialSynchronization
{
    using System;
    /// <summary>
    /// Indicates whether a site currently holds the token.
    /// </summary>
    public enum TokenState
    {
        WaitingForToken,
        HavingToken
    };
}