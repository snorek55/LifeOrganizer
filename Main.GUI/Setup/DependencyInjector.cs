using AutoMapper.Configuration;

using Common.Adapters;
using Common.Setup;
using Common.WPF;

using Main.GUI.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Main.GUI.Setup
{
	public class DependencyInjector : IInjector
	{
		private ServiceProvider provider;
		public PluginManager PluginManager = new PluginManager();
		private App app;

		public DependencyInjector(App app)
		{
			this.app = app;
			var services = new ServiceCollection();
			ConfigureServices(services);
			provider = services.BuildServiceProvider();
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton(app);
			LoadPlugins(services);
			LoadMain(services);
			services.AddSingleton<IAutoMapper, MapperImpl>(
				x =>
				{
					var configExpression = new MapperConfigurationExpression();

					foreach (var profileData in x.GetServices<IProfilePluginData>())
					{
						configExpression.AddProfiles(profileData.Profiles);
					}
					return new MapperImpl(configExpression);
				});
		}

		private void LoadPlugins(IServiceCollection services)
		{
			PluginManager.LoadPlugins(services, ConfigurationManager.AppSettings["MovOrgInfrastructurePath"], ConfigurationManager.AppSettings["MovOrgInfrastructureFile"]);
			PluginManager.LoadPlugins(services, ConfigurationManager.AppSettings["MovOrgOrganizerPath"], ConfigurationManager.AppSettings["MovOrgOrganizerFile"]);
		}

		private void LoadMain(IServiceCollection services)
		{
			services.AddSingleton(
				x =>
				{
					var mainVM = new MainWindowViewModel();

					foreach (var containerData in x.GetServices<IContainerPluginData>())
					{
						x.GetService<App>().Resources.MergedDictionaries.Add(new ResourceDictionary
						{
							Source = containerData.ResourceDictionaryUri
						});
						mainVM.MainMenuContainers.Add(containerData);
					}
					mainVM.SelectedItem = mainVM.MainMenuContainers.FirstOrDefault();

					return mainVM;
				});
			services.AddSingleton<MainWindow>();
			services.AddSingleton<IDispatcher, GuiDispatcher>();
		}

		public T Get<T>()
		{
			var service = provider.GetService<T>();
			if (service == null)
				throw new ArgumentException($"Service {nameof(T)} not found.");
			return service;
		}

		public IEnumerable<T> GetMany<T>()
		{
			var services = provider.GetServices<T>();
			if (services.Count() == 0)
				throw new ArgumentException($"There are no {nameof(T)} loaded.");

			return services;
		}
	}
}