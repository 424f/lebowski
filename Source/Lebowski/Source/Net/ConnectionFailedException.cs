using System;

namespace Lebowski.Net
{
    /// <summary>
    /// The exception that is thrown when a connection attempt by an 
    /// <see cref="IConnection">IConnection</see> has failed.
    /// </summary>
    public class ConnectionFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ConnectionFailedException
        /// with a message describing the error.
        /// </summary>
        /// <param name="message">See <see cref="Exception.Message">Message</see>.</param>
        public ConnectionFailedException(string message) : base(message) {}
        
        /// <summary>
        /// Initializes a new instance of the ConnectionFailedException with
        /// a message describing the error and the original exception that
        /// provides more information about the origin of the exception.
        /// </summary>
        /// <param name="message">See <see cref="Exception.Message">Message</see>.</param>        
        /// <param name="cause">See <see cref="Exception.InnerException">InnerException</see>.</param>        
        public ConnectionFailedException(string message, Exception cause) : base(message, cause) {}
    }
}
