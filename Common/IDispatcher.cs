using System;

namespace Common
{
	public interface IDispatcher
	{
		void BeginInvoke(Action a);
	}
}