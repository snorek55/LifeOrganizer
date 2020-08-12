using Common.Adapters;
using Common.Setup;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizer.Setup;
using MovOrg.Organizers.Adapters.Container;
using MovOrg.Organizers.Adapters.Sections;
using MovOrg.Organizers.UseCases;

namespace MovOrg.Organizers.Setup
{
	public class DependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IProfilePluginData, ProfilePluginData>();
			services.AddSingleton<IContainerPluginData, ContainerPluginData>();
			services.AddSingleton<IMoviesService, MoviesService>();
			services.AddSingleton<ISectionsFactory, MovOrgSectionsFactory>();
			services.AddSingleton<MovOrgContainerViewModel>();
			services.AddSingleton<ContainerPluginData>();

			services.AddSingleton<MoviesSectionViewModel>();
			services.AddSingleton<ActorsSectionViewModel>();
		}
	}
}