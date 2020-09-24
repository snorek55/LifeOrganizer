using Common.UseCases;
using Common.UseCases.Runners;

using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Movies.Requests;
using MovOrg.Organizer.UseCases.Requests;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class MoviesService : ServiceActionBase, IMoviesService
	{
		private readonly RunnerReadWriteDbAsync<GetMovieDetailsRequest, MovieWithDetailsDto> runnerMovieDetails;
		private readonly RunnerReadDbAsync<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>> runnerMoviesFromLocal;
		private readonly RunnerReadDbAsync<GetMovieImagesRequest, IEnumerable<MovieImageDto>> runnerMovieImages;
		private readonly RunnerWriteDbAsync<UpdateUserMovieStatusRequest> runnerUserMovieStatus;
		private readonly RunnerReadWriteDbAsync<GetMovieRatingSourceUrlRequest, MovieRatingSourceUrlDto> runnerGetRatingSourceUrl;
		private readonly RunnerReadWriteDbAsync<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>> runnerMoviesWithSuggestedTitle;
		private readonly RunnerWriteDbAsync<UpdateTopMoviesRequest> runnerUpdateTopMovies;

		public MoviesService(
			RunnerReadWriteDbAsync<GetMovieDetailsRequest, MovieWithDetailsDto> runnerMovieDetails,
			RunnerReadDbAsync<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>> runnerMoviesFromLocal,
			RunnerReadDbAsync<GetMovieImagesRequest, IEnumerable<MovieImageDto>> runnerMovieImages,
			RunnerWriteDbAsync<UpdateUserMovieStatusRequest> runnerUserMovieStatus,
			RunnerReadWriteDbAsync<GetMovieRatingSourceUrlRequest, MovieRatingSourceUrlDto> runnerGetRatingSourceUrl,
			RunnerReadWriteDbAsync<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>> runnerMoviesWithSuggestedTitle,
			 RunnerWriteDbAsync<UpdateTopMoviesRequest> runnerUpdateTopMovies
			)
		{
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

		public async Task<DataResponseBase<MovieRatingSourceUrlDto>> GetRatingSourceUrl(string id, string sourceName)
		{
			var ratingSourceUrl = await runnerGetRatingSourceUrl.RunAction(new GetMovieRatingSourceUrlRequest(id, sourceName));
			return ReturnErrorIfNull(ratingSourceUrl);
		}
	}
}