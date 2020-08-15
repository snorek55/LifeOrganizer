using Common.Adapters;
using Common.Setup;
using Common.WPF;

using Main.GUI.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Main.GUI.Setup
{
	public class DependencyInjector : IInjector
	{
		private ServiceProvider provider;
		public PluginManager PluginManager = new PluginManager();

		public DependencyInjector()
		{
			var services = new ServiceCollection();
			ConfigureServices(services);
			provider = services.BuildServiceProvider();
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton<IAutoMapper, MapperImpl>();
			LoadMain(services);
			LoadPlugins(services);
		}

		private void LoadPlugins(IServiceCollection services)
		{
			PluginManager.LoadPlugins(services, ConfigurationManager.AppSettings["MovOrgInfrastructurePath"], ConfigurationManager.AppSettings["MovOrgInfrastructureFile"]);
			PluginManager.LoadPlugins(services, ConfigurationManager.AppSettings["MovOrgOrganizerPath"], ConfigurationManager.AppSettings["MovOrgOrganizerFile"]);
		}

		private void LoadMain(IServiceCollection services)
		{
			services.AddSingleton<MainWindowViewModel>();
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