using System;

namespace Lebowski.Synchronization.DifferentialSynchronization
{
    /// <summary>
    /// Indicates whether a site currently holds the token.
    /// </summary>
	public enum TokenState
	{
		WaitingForToken,
		HavingToken
	};
}
