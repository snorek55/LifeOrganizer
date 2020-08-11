using Common.Adapters;
using Common.Config;
using Common.WPF;

using DesktopGui;
using DesktopGui.Main;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using EntryPoint.Mapper;

using Infrastructure.MovOrg.APIs;
using Infrastructure.MovOrg.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizers.Adapters.Container;
using MovOrg.Organizers.Adapters.Sections;
using MovOrg.Organizers.UseCases;
using MovOrg.Organizers.UseCases.Repositories;

using Organizers;
using Organizers.Main.Adapters;
using Organizers.Main.Adapters.MainMenu;

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
				var dependency = provider.GetRequiredService<IConfig>();
				var dbOptions = new DbContextOptionsBuilder<MoviesContext>().UseSqlServer(dependency.GetConnectionString())
						.EnableSensitiveDataLogging();

				var dbContextFactory = new DbContextFactory(dbOptions);

				return new DbContextScopeFactory(dbContextFactory);
			});
		}

		public void LoadAdapters(ServiceCollection services)
		{
			services.AddSingleton<ISectionsFactory, MovOrgSectionsFactory>();
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