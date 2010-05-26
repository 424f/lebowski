
using System;

namespace Lebowski.Net
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message) : base(message) {}
        public ConnectionFailedException(string message, Exception cause) : base(message, cause) {}
    }
}
