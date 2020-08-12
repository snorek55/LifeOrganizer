using DesktopGui.Main;

using EntryPoint.Setup;

using MovOrg.Organizers.Adapters.Container;

using Organizers;

using System;
using System.Linq;
using System.Windows;

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
			injector.App.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"pack://application:,,,/MovOrg.GUI;component/MovOrgDataTemplates.xaml") });

			var mainVM = injector.Get<MainWindowViewModel>();

			mainVM.MainMenuItems.Add(injector.Get<MovOrgMainMenuItemViewModel>());

			mainVM.SelectedItem = mainVM.MainMenuItems.FirstOrDefault();

			injector.App.Run(injector.Get<MainWindow>());
		}
	}
}