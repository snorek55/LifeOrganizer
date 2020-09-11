using Common.Adapters;
using Common.Setup;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizer.Adapters.Container;
using MovOrg.Organizer.Adapters.Sections;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Requests;
using MovOrg.Organizer.UseCases.Runners;

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MovOrg.Organizer.Setup
{
	//TODO: split adapters dependencies from usecases dependencies
	[Export(typeof(IDependencyResolver))]
	public class DependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			services.AddSingleton<IProfilePluginData, ProfilePluginData>();
			services.AddSingleton<IContainerPluginData, ContainerPluginData>();
			services.AddSingleton<IMoviesService, MoviesService>();
			services.AddSingleton<ISectionsFactory, MovOrgSectionsFactory>();

			services.AddSingleton<RunnerReadWriteDb<GetMovieDetailsRequest, MovieWithDetailsDto>>();
			services.AddSingleton<IServiceAction<GetMovieDetailsRequest, MovieWithDetailsDto>, GetMovieDetailsAction>();

			services.AddSingleton<RunnerReadDb<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>>>();
			services.AddSingleton<IServiceAction<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>>, GetMoviesFromLocalAction>();

			services.AddSingleton<RunnerReadDb<GetMovieImagesRequest, IEnumerable<MovieImageDto>>>();
			services.AddSingleton<IServiceAction<GetMovieImagesRequest, IEnumerable<MovieImageDto>>, GetMovieImagesAction>();

			services.AddSingleton<MovOrgContainerViewModel>();
			services.AddSingleton<ContainerPluginData>();

			services.AddSingleton<MoviesSectionViewModel>();
			services.AddSingleton<ActorsSectionViewModel>();
		}
	}
}