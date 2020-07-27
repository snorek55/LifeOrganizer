using DesktopGui;
using DesktopGui.Common;
using DesktopGui.Main;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using EntryPoint.Mapper;

using Infrastructure.EFCore;
using Infrastructure.MovOrg.APIs;
using Infrastructure.MovOrg.EFCore;

using Microsoft.Extensions.DependencyInjection;

using Organizers.Common;
using Organizers.Common.Config;
using Organizers.Main.Adapters;
using Organizers.Main.Adapters.MainMenu;
using Organizers.Main.Adapters.Sections;
using Organizers.MovOrg.Adapters.Container;
using Organizers.MovOrg.Adapters.Sections;
using Organizers.MovOrg.UseCases;
using Organizers.MovOrg.UseCases.Repositories;

namespace EntryPoint.DI
{
	internal class DependencyInjector : IInjector
	{
		private ServiceProvider provider;
		private IConfig config = new Config();
		public App App = new App();

		public DependencyInjector()
		{
			ServiceCollection services = new ServiceCollection();
			ConfigureServices(services);
			provider = services.BuildServiceProvider();
		}

		private void ConfigureServices(ServiceCollection services)
		{
			LoadRepositories(services);
			LoadApplication(services);
			LoadAdapters(services);
		}

		public T Get<T>()
		{
			return provider.GetService<T>();
		}

		private void LoadRepositories(ServiceCollection services)
		{
			services.AddSingleton<IApiMoviesRepository, IMDbMoviesApiRepository>();
			services.AddSingleton<ILocalMoviesRepository, EFCoreLocalMoviesRepository>();

			services.AddSingleton(config);
			services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
		}

		private void LoadApplication(ServiceCollection services)
		{
			services.AddSingleton<IAutoMapper, MapperImpl>();
			services.AddSingleton<IMoviesService, MoviesService>();
			services.AddDbContext<MoviesContext>();

			services.AddSingleton<DbContextFactory>();
			services.AddSingleton<IDbContextScopeFactory, DbContextScopeFactory>();

			services.AddSingleton<IDbContextScopeFactory>(provider =>
			{
				var dependency = provider.GetRequiredService<DbContextFactory>();
				return new DbContextScopeFactory(dependency);
			});
		}

		public void LoadAdapters(ServiceCollection services)
		{
			services.AddSingleton<IMainMenuItemFactory, MainMenuItemFactory>();
			services.AddSingleton<ISectionsFactory, SectionsFactory>();

			services.AddSingleton<MovOrgContainerViewModel>();
			services.AddSingleton<MainWindowViewModel>();
			services.AddSingleton<MoviesSectionViewModel>();
			services.AddSingleton<ActorsSectionViewModel>();

			services.AddSingleton<MainWindow>();
			services.AddSingleton(App);

			services.AddSingleton<IDispatcher>(new GuiDispatcher());
		}
	}
}