using DesktopGui.Main;

using EntryPoint.DI;

using System;

namespace WinFormsUI
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			var injector = new DependencyInjector();
			injector.App.Run(injector.Get<MainWindow>());
		}
	}
}