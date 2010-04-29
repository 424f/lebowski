
using System;

namespace Lebowski.Net
{
	public interface ICommunicationProtocol
	{
		string Name { get; }
		void Share(ISessionContext session);
	}
}
