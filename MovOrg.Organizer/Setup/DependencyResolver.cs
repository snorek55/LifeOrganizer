using Common.Adapters;
using Common.Setup;
using Common.UseCases;
using Common.UseCases.Runners;

using Microsoft.Extensions.DependencyInjection;

using MovOrg.Organizer.Adapters.Container;
using MovOrg.Organizer.Adapters.Sections;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Movies.Requests;
using MovOrg.Organizer.UseCases.Requests;

using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MovOrg.Organizer.Setup
{
	[Export(typeof(IDependencyResolver))]
	public class DependencyResolver : IDependencyResolver
	{
		public void Setup(IServiceCollection services)
		{
			SetupDependencies(services);
			UseCasesDependencies(services);
			AdaptersDependencies(services);
		}

		private void AdaptersDependencies(IServiceCollection services)
		{
			services.AddSingleton<MovOrgContainerViewModel>();
			services.AddSingleton<MoviesSectionViewModel>();
			services.AddSingleton<ActorsSectionViewModel>();
		}

		private void SetupDependencies(IServiceCollection services)
		{
			services.AddSingleton<IProfilePluginData, ProfilePluginData>();
			services.AddSingleton<IContainerPluginData, ContainerPluginData>();
			services.AddSingleton<ISectionsFactory, MovOrgSectionsFactory>();
		}

		private void UseCasesDependencies(IServiceCollection services)
		{
			services.AddSingleton<IMoviesService, MoviesService>();
			services.AddSingleton<RunnerReadWriteDbAsync<GetMovieDetailsRequest, MovieWithDetailsDto>>();
			services.AddSingleton<IServiceActionAsync<GetMovieDetailsRequest, MovieWithDetailsDto>, GetMovieDetailsAction>();

			services.AddSingleton<RunnerReadDbAsync<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>>>();
			services.AddSingleton<IServiceActionAsync<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>>, GetMoviesFromLocalAction>();

			services.AddSingleton<RunnerReadDbAsync<GetMovieImagesRequest, IEnumerable<MovieImageDto>>>();
			services.AddSingleton<IServiceActionAsync<GetMovieImagesRequest, IEnumerable<MovieImageDto>>, GetMovieImagesAction>();

			services.AddSingleton<RunnerWriteDbAsync<UpdateUserMovieStatusRequest>>();
			services.AddSingleton<IServiceActionAsync<UpdateUserMovieStatusRequest>, UpdateUserMovieStatusAction>();

			services.AddSingleton<RunnerReadWriteDbAsync<GetMovieRatingSourceUrlRequest, MovieRatingSourceUrlDto>>();
			services.AddSingleton<IServiceActionAsync<GetMovieRatingSourceUrlRequest, MovieRatingSourceUrlDto>, GetMovieRatingSourceUrlAction>();

			services.AddSingleton<RunnerReadWriteDbAsync<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>>>();
			services.AddSingleton<IServiceActionAsync<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>>, GetMoviesFromSuggestedTitleAction>();

			services.AddSingleton<RunnerWriteDbAsync<UpdateTopMoviesRequest>>();
			services.AddSingleton<IServiceActionAsync<UpdateTopMoviesRequest>, UpdateTopMoviesAction>();
		}
	}
}