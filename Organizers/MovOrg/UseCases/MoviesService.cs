using Domain;

using EntityFramework.DbContextScope.Interfaces;

using Organizers.Common.Config;
using Organizers.Common.UseCases;
using Organizers.MovOrg.UseCases.Repositories;
using Organizers.MovOrg.UseCases.Responses;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizers.MovOrg.UseCases
{
	public class MoviesService : IMoviesService
	{
		private IDbContextScopeFactory dbContextScopeFactory;
		private ILocalMoviesRepository localRepository;
		private IApiMoviesRepository apiRepository;
		private IConfig config;

		public MoviesService(IDbContextScopeFactory dbContextScopeFactory, ILocalMoviesRepository localRepository, IApiMoviesRepository apiRepository, IConfig config)
		{
			this.dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException(nameof(dbContextScopeFactory));
			this.localRepository = localRepository ?? throw new ArgumentNullException(nameof(localRepository));
			this.apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
			this.config = config ?? throw new ArgumentNullException(nameof(config));
		}

		public async Task<GetAllMoviesFromLocalResponse> GetAllMoviesFromLocal()
		{
			try
			{
				using var context = dbContextScopeFactory.CreateReadOnly();
				var movies = await localRepository.GetAllMovies();
				return new GetAllMoviesFromLocalResponse(movies);
			}
			catch (RepositoryException ex)
			{
				return new GetAllMoviesFromLocalResponse(ex.ToString());
			}
		}

		public async Task<GetSuggestedTitleMoviesResponse> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false)
		{
			try
			{
				var wasSearched = await config.WasAlreadySearched(suggestedTitle);
				if (wasSearched)
					throw new RepositoryException("Word already looked up, please filter movies");

				using var context = dbContextScopeFactory.Create();
				IEnumerable<Movie> movies;
				movies = await apiRepository.GetMoviesFromSuggestedTitle(suggestedTitle);
				await localRepository.UpdateSuggestedTitleMovies(movies);
				await context.SaveChangesAsync();
				return new GetSuggestedTitleMoviesResponse(movies);
			}
			catch (RepositoryException ex)
			{
				return new GetSuggestedTitleMoviesResponse(ex.ToString());
			}
		}

		public async Task<GetMovieDetailsResponse> GetMovieWithId(string id, bool forceUpdateFromApi = false)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				bool areDetailsAvailableInLocal = await localRepository.AreDetailsAvailableFor(id);
				Movie movie = null;
				if (areDetailsAvailableInLocal && !forceUpdateFromApi)
				{
					movie = await localRepository.GetMovieDetailsById(id);
				}
				else
				{
					movie = await apiRepository.GetMovieDetailsById(id);
					await localRepository.UpdateMovieDetails(movie);
					await context.SaveChangesAsync();
				}

				return new GetMovieDetailsResponse(movie);
			}
			catch (RepositoryException ex)
			{
				return new GetMovieDetailsResponse(ex.ToString());
			}
		}

		public UpdateFavoriteResponse UpdateFavoriteStatus(string id, bool isFavorite)
		{
			throw new System.NotImplementedException();
		}

		public UpdateMustWatchResponse UpdateMustWatch(string id, bool isMustWatch)
		{
			throw new System.NotImplementedException();
		}

		public async Task<UpdateTopMoviesResponse> UpdateTopMovies()
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				var topApiMovies = await apiRepository.GetTopMovies();

				await localRepository.UpdateTopMovies(topApiMovies);
				await context.SaveChangesAsync();
				return new UpdateTopMoviesResponse();
			}
			catch (RepositoryException ex)
			{
				return new UpdateTopMoviesResponse { Error = ex.ToString() };
			}
		}

		public UpdateWatchedResponse UpdateWatched(string id, bool isWatched)
		{
			throw new System.NotImplementedException();
		}
	}
}