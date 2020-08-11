using Organizers.Common;

using System;
using System.Windows;
using System.Windows.Threading;

namespace Common.WPF
{
	public class GuiDispatcher : IDispatcher
	{
		private Dispatcher dispatcher;

		public GuiDispatcher()
		{
			dispatcher = Application.Current.Dispatcher;
		}

		public void BeginInvoke(Action a)
		{
			dispatcher.BeginInvoke(a);
		}
	}
}