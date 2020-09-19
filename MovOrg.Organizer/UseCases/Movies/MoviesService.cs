using Common.Setup;
using Common.UseCases;

using EntityFramework.DbContextScope.Interfaces;

using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Movies.Requests;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Organizer.UseCases.Requests;
using MovOrg.Organizer.UseCases.Runners;

using System;
using System.Collections.Generic;
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
		private readonly RunnerReadDb<GetMovieImagesRequest, IEnumerable<MovieImageDto>> runnerMovieImages;
		private readonly RunnerWriteDb<UpdateUserMovieStatusRequest> runnerUserMovieStatus;
		private readonly RunnerReadWriteDb<GetMovieRatingSourceUrlRequest, MovieRatingSourceDto> runnerGetRatingSourceUrl;
		private readonly RunnerReadWriteDb<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>> runnerMoviesWithSuggestedTitle;
		private readonly RunnerWriteDb<UpdateTopMoviesRequest> runnerUpdateTopMovies;

		public MoviesService(
			IDbContextScopeFactory dbContextScopeFactory,
			ILocalMoviesRepository localRepository,
			IApiMoviesRepository apiRepository,
			IConfig config,
			RunnerReadWriteDb<GetMovieDetailsRequest, MovieWithDetailsDto> runnerMovieDetails,
			RunnerReadDb<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>> runnerMoviesFromLocal,
			RunnerReadDb<GetMovieImagesRequest, IEnumerable<MovieImageDto>> runnerMovieImages,
			RunnerWriteDb<UpdateUserMovieStatusRequest> runnerUserMovieStatus,
			RunnerReadWriteDb<GetMovieRatingSourceUrlRequest, MovieRatingSourceDto> runnerGetRatingSourceUrl,
			RunnerReadWriteDb<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>> runnerMoviesWithSuggestedTitle,
			 RunnerWriteDb<UpdateTopMoviesRequest> runnerUpdateTopMovies
			)
		{
			this.dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException(nameof(dbContextScopeFactory));
			this.localRepository = localRepository ?? throw new ArgumentNullException(nameof(localRepository));
			this.apiRepository = apiRepository ?? throw new ArgumentNullException(nameof(apiRepository));
			this.config = config ?? throw new ArgumentNullException(nameof(config));
			this.runnerMovieDetails = runnerMovieDetails;
			this.runnerMoviesFromLocal = runnerMoviesFromLocal;
			this.runnerMovieImages = runnerMovieImages;
			this.runnerUserMovieStatus = runnerUserMovieStatus;
			this.runnerGetRatingSourceUrl = runnerGetRatingSourceUrl;
			this.runnerMoviesWithSuggestedTitle = runnerMoviesWithSuggestedTitle;
			this.runnerUpdateTopMovies = runnerUpdateTopMovies;
		}

		public async Task<DataResponseBase<IEnumerable<MovieListItemDto>>> GetAllMoviesFromLocal()
		{
			var moviesFromLocal = await runnerMoviesFromLocal.RunAction(new GetMoviesFromLocalRequest());
			return ReturnErrorIfNull(moviesFromLocal);
		}

		public async Task<DataResponseBase<MovieWithDetailsDto>> GetMovieWithId(string id, bool forceUpdateFromApi = false)
		{
			var movieWithDetails = await runnerMovieDetails.RunAction(new GetMovieDetailsRequest(id));
			return ReturnErrorIfNull(movieWithDetails);
		}

		public async Task<DataResponseBase<IEnumerable<MovieImageDto>>> GetMovieImagesById(string id)
		{
			var images = await runnerMovieImages.RunAction(new GetMovieImagesRequest(id));
			return ReturnErrorIfNull(images);
		}

		public async Task<DataResponseBase<IEnumerable<MovieListItemDto>>> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false)
		{
			var moviesWithSuggestedTitle = await runnerMoviesWithSuggestedTitle.RunAction(new GetMoviesFromSuggestedTitleRequest(suggestedTitle));

			return ReturnErrorIfNull(moviesWithSuggestedTitle);
		}

		//TODO: poner nuevo sistema
		public async Task<ResponseBase> UpdateTopMovies()
		{
			var isOk = await runnerUpdateTopMovies.RunAction(new UpdateTopMoviesRequest());
			return ReturnErrorIfFalse(isOk);
		}

		public async Task<ResponseBase> UpdateFavoriteStatus(string id, bool isFavorite)
		{
			var isOk = await runnerUserMovieStatus.RunAction(new UpdateUserMovieStatusRequest(MovieUserStatus.IsFavorite, id, isFavorite));

			return ReturnErrorIfFalse(isOk);
		}

		public async Task<ResponseBase> UpdateMustWatch(string id, bool isMustWatch)
		{
			var isOk = await runnerUserMovieStatus.RunAction(new UpdateUserMovieStatusRequest(MovieUserStatus.MustWatch, id, isMustWatch));

			return ReturnErrorIfFalse(isOk);
		}

		public async Task<ResponseBase> UpdateWatched(string id, bool isWatched)
		{
			var isOk = await runnerUserMovieStatus.RunAction(new UpdateUserMovieStatusRequest(MovieUserStatus.Watched, id, isWatched));

			return ReturnErrorIfFalse(isOk);
		}

		public async Task<DataResponseBase<MovieRatingSourceDto>> GetRatingSourceUrl(string id, string sourceName)
		{
			var ratingSourceUrl = await runnerGetRatingSourceUrl.RunAction(new GetMovieRatingSourceUrlRequest(id, sourceName));
			return ReturnErrorIfNull(ratingSourceUrl);
		}
	}
}