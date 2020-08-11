using Common.Adapters;
using Common.DI;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizers.Adapters.Container;
using MovOrg.Organizers.Adapters.Sections;
using MovOrg.Organizers.UseCases;

namespace MovOrg.Organizers.DI
{
	public class MovOrgOrganizerDependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IMoviesService, MoviesService>();
			services.AddSingleton<ISectionsFactory, MovOrgSectionsFactory>();
			services.AddSingleton<MovOrgContainerViewModel>();
			services.AddSingleton<MovOrgMainMenuItemViewModel>();

			services.AddSingleton<MoviesSectionViewModel>();
			services.AddSingleton<ActorsSectionViewModel>();
		}
	}
}