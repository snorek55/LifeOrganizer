using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public interface IMoviesService
	{
		Task<DataResponseBase<IEnumerable<MovieListItemDto>>> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false);

		Task<ResponseBase> UpdateTopMovies();

		Task<DataResponseBase<MovieWithDetailsDto>> GetMovieWithId(string id, bool forceUpdateFromApi = false);

		Task<DataResponseBase<IEnumerable<MovieImageDto>>> GetMovieImagesById(string id);

		Task<DataResponseBase<IEnumerable<MovieListItemDto>>> GetAllMoviesFromLocal();

		Task<ResponseBase> UpdateFavoriteStatus(string id, bool isFavorite);

		Task<ResponseBase> UpdateMustWatch(string id, bool isMustWatch);

		Task<ResponseBase> UpdateWatched(string id, bool isWatched);

		Task<DataResponseBase<MovieRatingSourceUrlDto>> GetRatingSourceUrl(string id, string sourceName);
	}
}