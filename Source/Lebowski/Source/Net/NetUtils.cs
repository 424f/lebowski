using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lebowski.Net
{
	public static class NetUtils
	{
		public static byte[] Serialize(object o)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, o);
			ms.Close();
			return ms.ToArray();
		}
		
		public static object Deserialize(byte[] buffer)
		{
			MemoryStream ms = new MemoryStream();
			int length = buffer.Length;
			ms.Write(buffer, 0, length);
			ms.Seek(0, SeekOrigin.Begin);
			
			BinaryFormatter formatter = new BinaryFormatter();
			return formatter.Deserialize(ms);
		}
	}
}
