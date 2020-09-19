using Common.Setup;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Infrastructure.APIs;
using MovOrg.Infrastructure.EFCore;
using MovOrg.Infrastructure.EFCore.DbAccess;
using MovOrg.Organizer.UseCases.DbAccess;

using System.ComponentModel.Composition;

namespace MovOrg.Infrastructure.Setup
{
	[Export(typeof(IDependencyResolver))]
	public class DependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IProfilePluginData, ProfilePluginData>();
			services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
			services.AddDbContext<MoviesContext>();
			services.AddSingleton<IMovieDetailsDbAccess, MovieDetailsDbAccess>();
			services.AddSingleton<IMovieDetailsApiAccess, MovieDetailsApiAccess>();
			services.AddSingleton<IMoviesListsDbAccess, MoviesListsDbAccess>();
			services.AddSingleton<IMoviesListsApiAccess, MoviesListsApiAccess>();

			services.AddSingleton<IDbContextScopeFactory>(provider =>
			{
				var dependency = provider.GetRequiredService<IConfig>();
				var dbContextFactory = new DbContextFactory(dependency);
				return new DbContextScopeFactory(dbContextFactory);
			});
		}
	}
}