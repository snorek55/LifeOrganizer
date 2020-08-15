using Common.Adapters;
using Common.Setup;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizer.Adapters.Container;
using MovOrg.Organizer.Adapters.Sections;
using MovOrg.Organizer.UseCases;

using System.ComponentModel.Composition;

namespace MovOrg.Organizer.Setup
{
	[Export(typeof(IDependencyResolver))]
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