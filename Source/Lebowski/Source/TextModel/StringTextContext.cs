using System;

namespace Lebowski.TextModel
{
	public class StringTextContext : ITextContext
	{
		public event EventHandler<EventArgs> Inserted;
		
		public event EventHandler<EventArgs> Deleted;
		
		public string Data { get; set; }
		
		public StringTextContext()
		{
			Data = "";
		}
		
		public void Insert(string text, int position)
		{
			Data = Data.Substring(0, position) + text + Data.Substring(position);
		}
		
		public void Delete(int position, int length)
		{
			Data = Data.Substring(0, position) + Data.Substring(position+length);
		}
	}
}
