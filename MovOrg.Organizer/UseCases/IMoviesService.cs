using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Responses;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public interface IMoviesService
	{
		Task<GetSuggestedTitleMoviesResponse> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false);

		Task<UpdateTopMoviesResponse> UpdateTopMovies();

		Task<DataResponseBase<MovieWithDetailsDto>> GetMovieWithId(string id, bool forceUpdateFromApi = false);

		GetMovieImagesResponse GetMovieImagesById(string id);

		Task<DataResponseBase<IEnumerable<MovieListItemDto>>> GetAllMoviesFromLocal();

		UpdateFavoriteResponse UpdateFavoriteStatus(string id, bool isFavorite);

		UpdateMustWatchResponse UpdateMustWatch(string id, bool isMustWatch);

		UpdateWatchedResponse UpdateWatched(string id, bool isWatched);

		Task<GetRatingSourceUrlResponse> GetRatingSourceUrl(string id, string sourceName);
	}
}