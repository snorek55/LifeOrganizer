using System;

namespace Organizers.Common
{
	public interface IDispatcher
	{
		void BeginInvoke(Action a);
	}
}