using Common.Setup;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MovOrg.Infrastructure.MovOrg.APIs;
using MovOrg.Infrastructure.MovOrg.EFCore;
using MovOrg.Organizer.UseCases.Repositories;

using System.ComponentModel.Composition;

namespace MovOrg.Infrastructure.Setup
{
	[Export(typeof(IDependencyResolver))]
	public class DependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IProfilePluginData, ProfilePluginData>();
			services.AddSingleton<IApiMoviesRepository, IMDbMoviesApiRepository>();
			services.AddSingleton<ILocalMoviesRepository, EFCoreLocalMoviesRepository>();
			services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
			services.AddDbContext<MoviesContext>();

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