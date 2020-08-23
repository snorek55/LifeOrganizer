using Common.Setup;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Infrastructure.APIs;
using MovOrg.Infrastructure.EFCore;
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
				var dbContextFactory = new DbContextFactory(dependency);
				return new DbContextScopeFactory(dbContextFactory);
			});
		}
	}
}