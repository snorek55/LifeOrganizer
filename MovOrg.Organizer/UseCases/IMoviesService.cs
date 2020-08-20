using MovOrg.Organizer.UseCases.Responses;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public interface IMoviesService
	{
		Task<GetSuggestedTitleMoviesResponse> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false);

		Task<UpdateTopMoviesResponse> UpdateTopMovies();

		Task<GetMovieDetailsResponse> GetMovieWithId(string id, bool forceUpdateFromApi = false);

		GetMovieImagesResponse GetMovieImagesById(string id);

		Task<GetAllMoviesFromLocalResponse> GetAllMoviesFromLocal();

		UpdateFavoriteResponse UpdateFavoriteStatus(string id, bool isFavorite);

		UpdateMustWatchResponse UpdateMustWatch(string id, bool isMustWatch);

		UpdateWatchedResponse UpdateWatched(string id, bool isWatched);
	}
}