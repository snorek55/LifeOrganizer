using AutoMapper.Configuration;

using Common.Setup;

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
			var autoMapper = injector.Get<IAutoMapper>();

			var infrastructureProfileData = new MovOrg.Infrastructure.Setup.ProfilePluginData();
			var movOrgProfileData = new MovOrg.Organizer.Setup.ProfilePluginData();
			var movOrgContainerData = injector.Get<ContainerPluginData>();

			var configExpression = new MapperConfigurationExpression();
			configExpression.AddProfiles(movOrgProfileData.Profiles);
			configExpression.AddProfiles(infrastructureProfileData.Profiles);
			autoMapper.CreateMapper(configExpression);

			var mainVM = injector.Get<MainWindowViewModel>();
			var mainMenuItem = injector.Get<ContainerPluginData>();
			injector.App.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = movOrgContainerData.ResourceDictionaryUri });

			mainVM.MainMenuItems.Add(mainMenuItem);

			mainVM.SelectedItem = mainVM.MainMenuItems.FirstOrDefault();

			injector.App.Run(injector.Get<MainWindow>());
		}
	}
}