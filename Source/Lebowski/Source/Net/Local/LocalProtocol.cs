namespace Lebowski.Net.Local
{
    /// <summary>
    /// Provides functionality to connect LocalConnections
    /// </summary>
    public static class LocalProtocol
    {
        /// <summary>
        /// Connects two LocalConnections, ensuring that each of them has the
        /// other one as the endpoint.
        /// </summary>
        /// <param name="first">The first connection to connect</param>
        /// <param name="second">The second connection to connect</param>
        public static void Connect(LocalConnection first, LocalConnection second)
        {
            first.Endpoint = second;
            second.Endpoint = first;
        }
    }
}