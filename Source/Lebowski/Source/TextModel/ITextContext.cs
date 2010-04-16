using System;

namespace Lebowski.TextModel
{
	public interface ITextContext
	{
		string Data { get; set; }
		
		void Insert(string text, int position);
		void Delete(int position, int length);
		
		event EventHandler<EventArgs> Inserted;
		event EventHandler<EventArgs> Deleted;
	}
}
