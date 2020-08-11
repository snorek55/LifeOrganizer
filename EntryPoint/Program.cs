using DesktopGui.Main;

using EntryPoint.DI;

using MovOrg.Organizers.Adapters.Container;

using Organizers;
using Organizers.Main.Adapters.MainMenu;

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

			var movOrgMainMenuItem = new MainMenuItemViewModel("Movies", MainMenuIconType.Movies, injector.Get<MovOrgContainerViewModel>());

			mainVM.MainMenuItems.Add(movOrgMainMenuItem);

			mainVM.SelectedItem = mainVM.MainMenuItems.FirstOrDefault();

			injector.App.Run(injector.Get<MainWindow>());
		}
	}
}