using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Lebowski.Test.Net
{
	[Serializable]
	class Data
	{
		public string Name;
		public string Email { get; set; }
	};
	
	[TestFixture]
	public class EncodingTest
	{
		public EncodingTest()
		{
		}
		
		[Test]
		public void TestBase64()
		{
			var data = new Data();
			data.Name = "Boris Bluntschli";
			data.Email = "boris@bluntschli.ch";

			// Serialize
			var stream = new MemoryStream();
			var formatter = new BinaryFormatter();
			formatter.Serialize(stream, data);
			string base64 = Convert.ToBase64String(stream.GetBuffer());
			
			// Deserialize
			stream = new MemoryStream(Convert.FromBase64String(base64));
			object obj = formatter.Deserialize(stream);
			Console.WriteLine(obj);
		}
	}
}
