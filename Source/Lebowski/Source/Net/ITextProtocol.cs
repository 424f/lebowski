using System;

namespace Lebowski.Net
{
	/// <summary>
	/// Defines a protocol that can be used to connect to a remote machine
	/// and exchange text messages with it.
	/// 
	/// The sender has to guarantee that:
	/// 	- the message does *not* contain a null character
	/// 
	/// The protocol has to guarantee that:
	/// 	- For each Send() call, on the remote end, OnReceive is triggered
	/// exactly once
	/// </summary>
	public interface ITextProtocol : IDisposable
	{
		string Name { get; }
		
		ITextConnection Connect(object settings);
	}
	
	public interface ITextConnection
	{
		void Send(string message);
		event EventHandler<EventArgs> Received;		
	}
}
