namespace Lebowski.Net
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;
    
    /// <summary>
    /// Provides helper methods for networking
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        /// Serializes an object to a byte array
        /// </summary>
        /// <param name="o">object to serialize</param>
        /// <returns>Bytes representing the serialized object</returns>
        public static byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, o);
            ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Deserializes an object from a byte array
        /// </summary>
        /// <param name="buffer">The byte array (usually previously obtained using <see cref="Serialize">Serialize</see>)</param>
        /// <returns>The deserialized object.</returns>
        public static object Deserialize(byte[] buffer)
        {
            return Deserialize(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Deserializes an object from a certain range of a byte array
        /// </summary>
        /// <param name="buffer">The byte array (usually previously obtained using <see cref="Serialize">Serialize</see>)</param>
        /// <param name="offset">The offset of the first byte that is part of the object data.</param>
        /// <param name="count">The number of bytes, starting at offset, that are part of the object data.</param>
        /// <returns>The deserialized object.</returns>
        public static object Deserialize(byte[] buffer, int offset, int count)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(buffer, offset, count);
            ms.Seek(0, SeekOrigin.Begin);

            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(ms);
        }

        /// <summary>
        /// Reads a fixed number of bytes from a network stream. If not enough
        /// data is currently available, this method blocks until the desired
        /// number of bytes has been read.
        /// </summary>
        /// <param name="stream">The NetworkStream to read from.</param>
        /// <param name="numBytes">The number of bytes that should be read.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(NetworkStream stream, int numBytes)
        {
            byte[] buffer = new byte[numBytes];
            int bytesRead = 0;
            while (bytesRead < numBytes)
            {
                bytesRead += stream.Read(buffer, bytesRead, numBytes-bytesRead);
            }
            return buffer;
        }
    }
}