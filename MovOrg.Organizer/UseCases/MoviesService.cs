using Common.Setup;
using Common.UseCases;

using EntityFramework.DbContextScope.Interfaces;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Organizer.UseCases.Responses;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
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
				if (!wasSearched || forceUpdateFromApi)
					return await LoadMoviesFromSuggestedTitleAsync(suggestedTitle, forceUpdateFromApi);
				else
					return new GetSuggestedTitleMoviesResponse
					{
						AlreadySearched = true
					};
			}
			catch (Exception ex)
			{
				if (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is RepositoryException)
					return new GetSuggestedTitleMoviesResponse(ex.ToString());

				throw;
			}
		}

		private async Task<GetSuggestedTitleMoviesResponse> LoadMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi)
		{
			using var context = dbContextScopeFactory.Create();
			IEnumerable<Movie> movies;
			movies = await apiRepository.GetMoviesFromSuggestedTitle(suggestedTitle);
			await localRepository.UpdateSuggestedTitleMovies(movies);
			await context.SaveChangesAsync();
			await config.AddSearchedTitleAsync(suggestedTitle);
			return new GetSuggestedTitleMoviesResponse(movies);
		}

		public async Task<GetMovieDetailsResponse> GetMovieWithId(string id, bool forceUpdateFromApi = false)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				bool areDetailsAvailableInLocal = await localRepository.AreDetailsAvailableFor(id);
				Movie movie = null;
				if (!areDetailsAvailableInLocal || forceUpdateFromApi)
				{
					movie = await apiRepository.GetMovieDetailsById(id);
					await localRepository.UpdateMovieDetails(movie);
				}
				else
				{
					movie = await localRepository.GetMovieDetailsById(id);
				}
				await context.SaveChangesAsync();
				return new GetMovieDetailsResponse(movie);
			}
			catch (RepositoryException ex)
			{
				return new GetMovieDetailsResponse(ex.ToString());
			}
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

		public UpdateFavoriteResponse UpdateFavoriteStatus(string id, bool isFavorite)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				localRepository.MarkMovieAsFavorite(id, isFavorite);
				context.SaveChanges();
				return new UpdateFavoriteResponse();
			}
			catch (RepositoryException ex)
			{
				return new UpdateFavoriteResponse(ex.ToString());
			}
		}

		public UpdateMustWatchResponse UpdateMustWatch(string id, bool isMustWatch)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				localRepository.MarkMovieAsMustWatch(id, isMustWatch);
				context.SaveChanges();
				return new UpdateMustWatchResponse();
			}
			catch (RepositoryException ex)
			{
				return new UpdateMustWatchResponse(ex.ToString());
			}
		}

		public UpdateWatchedResponse UpdateWatched(string id, bool isWatched)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				localRepository.MarkMovieAsWatched(id, isWatched);
				context.SaveChanges();
				return new UpdateWatchedResponse();
			}
			catch (RepositoryException ex)
			{
				return new UpdateWatchedResponse(ex.ToString());
			}
		}
	}
}