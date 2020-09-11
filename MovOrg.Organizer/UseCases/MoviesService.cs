using Common.Setup;
using Common.UseCases;

using EntityFramework.DbContextScope.Interfaces;

using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Organizer.UseCases.Requests;
using MovOrg.Organizer.UseCases.Responses;
using MovOrg.Organizer.UseCases.Runners;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	//Adapts info from wpf to call business logic
	public class MoviesService : ServiceActionBase, IMoviesService
	{
		//TODO: tengo que pasarle varios runners pero la clave esta en que cuando termine de implementarlos podre suprimir este service e utilizar directamente los runners en los adapters
		private IDbContextScopeFactory dbContextScopeFactory;

		private ILocalMoviesRepository localRepository;
		private IApiMoviesRepository apiRepository;
		private IConfig config;
		private readonly RunnerReadWriteDb<GetMovieDetailsRequest, MovieWithDetailsDto> runnerMovieDetails;
		private readonly RunnerReadDb<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>> runnerMoviesFromLocal;

		public MoviesService(
			IDbContextScopeFactory dbContextScopeFactory,
			ILocalMoviesRepository localRepository,
			IApiMoviesRepository apiRepository,
			IConfig config,
			RunnerReadWriteDb<GetMovieDetailsRequest, MovieWithDetailsDto> runnerMovieDetails,
			RunnerReadDb<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>> runnerMoviesFromLocal
			)
		{
			this.dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException(nameof(dbContextScopeFactory));
			this.localRepository = localRepository ?? throw new ArgumentNullException(nameof(localRepository));
			this.apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
			this.config = config ?? throw new ArgumentNullException(nameof(config));
			this.runnerMovieDetails = runnerMovieDetails;
			this.runnerMoviesFromLocal = runnerMoviesFromLocal;
		}

		public async Task<GetMoviesFromLocalResponse> GetAllMoviesFromLocal()
		{
			var moviesFromLocal = await runnerMoviesFromLocal.RunAction(new GetMoviesFromLocalRequest());

			if (moviesFromLocal == null)
				return new GetMoviesFromLocalResponse(runnerMoviesFromLocal.Errors.ToString());
			else
				return new GetMoviesFromLocalResponse(moviesFromLocal);
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
			IEnumerable<MovieListItemDto> movies;
			movies = await apiRepository.GetMoviesFromSuggestedTitle(suggestedTitle);
			await localRepository.UpdateSuggestedTitleMovies(movies);
			await context.SaveChangesAsync();
			await config.AddSearchedTitleAsync(suggestedTitle);
			return new GetSuggestedTitleMoviesResponse(movies);
		}

		public async Task<GetMovieDetailsResponse> GetMovieWithId(string id, bool forceUpdateFromApi = false)
		{
			var movieWithDetails = await runnerMovieDetails.RunAction(new GetMovieDetailsRequest(id));

			if (movieWithDetails == null)
				return new GetMovieDetailsResponse(runnerMovieDetails.Errors.ToString());
			else
				return new GetMovieDetailsResponse(movieWithDetails);
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

		public GetMovieImagesResponse GetMovieImagesById(string id)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				var images = localRepository.GetMovieImagesById(id);
				return new GetMovieImagesResponse(images);
			}
			catch (RepositoryException ex)
			{
				return new GetMovieImagesResponse(ex.ToString());
			}
		}

		public async Task<GetRatingSourceUrlResponse> GetRatingSourceUrl(string id, string sourceName)
		{
			try
			{
				using var context = dbContextScopeFactory.Create();
				var sourcesWebPages = localRepository.GetRatingSourceUrls(id);

				var nullWebPages = sourcesWebPages.Where(x => x.SourceUrl == null);
				if (nullWebPages.Count() == sourcesWebPages.Count())
				{
					sourcesWebPages = await apiRepository.GetRatingSourcesUrls(id);
					localRepository.UpdateSourcesWebPages(id, sourcesWebPages);
					await context.SaveChangesAsync();
				}

				foreach (var source in sourcesWebPages)
				{
					if (source.SourceName == sourceName)
						return new GetRatingSourceUrlResponse(source);
				}

				return new GetRatingSourceUrlResponse();
			}
			catch (RepositoryException ex)
			{
				return new GetRatingSourceUrlResponse(ex.ToString());
			}
		}
	}
}