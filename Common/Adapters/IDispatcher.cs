using System;

namespace Common.Adapters
{
	public interface IDispatcher
	{
		void BeginInvoke(Action a);
	}
}