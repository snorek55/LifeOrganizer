using Common.Adapters;
using Common.Setup;
using Common.WPF;

using DesktopGui;
using DesktopGui.Main;

using EntryPoint.Mapper;

using Microsoft.Extensions.DependencyInjection;

using Organizers;

namespace EntryPoint.Setup
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

			var infraServices = new Infrastructure.Setup.DependencyResolver();
			var movServices = new MovOrg.Organizers.Setup.DependencyResolver();
			infraServices.Setup(services);
			movServices.Setup(services);
		}

		public void LoadMain(ServiceCollection services)
		{
			services.AddSingleton<MainWindowViewModel>();
			services.AddSingleton<MainWindow>();
			services.AddSingleton(App);
			services.AddSingleton<IDispatcher, GuiDispatcher>();
		}

		public T Get<T>()
		{
			return provider.GetService<T>();
		}
	}
}