﻿using Common.Adapters;
using Common.Config;
using Common.DI;
using Common.WPF;

using DesktopGui;
using DesktopGui.Main;

using EntryPoint.Mapper;

using Infrastructure.DI;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizers.DI;

using Organizers;

namespace EntryPoint.DI
{
	internal class DependencyInjector : IInjector
	{
		private ServiceProvider provider;
		public App App = new App();

		public DependencyInjector()
		{
			ServiceCollection services = new ServiceCollection();
			ConfigureServices(services);
			provider = services.BuildServiceProvider();
		}

		private void ConfigureServices(ServiceCollection services)
		{
			services.AddSingleton<IConfig, Config>();
			services.AddSingleton<IAutoMapper, MapperImpl>();
			LoadMain(services);

			var infraServices = new MovOrgInfrastructureDependencyResolver();
			var movServices = new MovOrgOrganizerDependencyResolver();
			infraServices.Setup(services);
			movServices.Setup(services);
		}

		public T Get<T>()
		{
			return provider.GetService<T>();
		}

		public void LoadMain(ServiceCollection services)
		{
			services.AddSingleton<MainWindowViewModel>();
			services.AddSingleton<MainWindow>();
			services.AddSingleton(App);
			services.AddSingleton<IDispatcher, GuiDispatcher>();
		}
	}
}