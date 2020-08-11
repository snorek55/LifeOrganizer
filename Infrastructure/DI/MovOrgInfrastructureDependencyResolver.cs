using Common.Config;
using Common.DI;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.MovOrg.APIs;
using Infrastructure.MovOrg.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizers.UseCases.Repositories;

namespace Infrastructure.DI
{
	public class MovOrgInfrastructureDependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IApiMoviesRepository, IMDbMoviesApiRepository>();
			services.AddSingleton<ILocalMoviesRepository, EFCoreLocalMoviesRepository>();
			services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
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
	}
}