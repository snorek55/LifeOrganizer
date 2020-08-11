using DesktopGui.Main;

using EntryPoint.DI;

using MovOrg.Organizers.Adapters.Container;

using Organizers;

using System;
using System.Linq;

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

			var mainVM = injector.Get<MainWindowViewModel>();

			mainVM.MainMenuItems.Add(injector.Get<MovOrgMainMenuItemViewModel>());

			mainVM.SelectedItem = mainVM.MainMenuItems.FirstOrDefault();

			injector.App.Run(injector.Get<MainWindow>());
		}
	}
}