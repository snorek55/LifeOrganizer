﻿using MovOrg.Organizers.UseCases.Responses;

using System.Threading.Tasks;

namespace MovOrg.Organizers.UseCases
{
	public interface IMoviesService
	{
		Task<GetSuggestedTitleMoviesResponse> GetMoviesFromSuggestedTitleAsync(string suggestedTitle, bool forceUpdateFromApi = false);

		Task<UpdateTopMoviesResponse> UpdateTopMovies();

		Task<GetMovieDetailsResponse> GetMovieWithId(string id, bool forceUpdateFromApi = false);

		Task<GetAllMoviesFromLocalResponse> GetAllMoviesFromLocal();

		UpdateFavoriteResponse UpdateFavoriteStatus(string id, bool isFavorite);

		UpdateMustWatchResponse UpdateMustWatch(string id, bool isMustWatch);

		UpdateWatchedResponse UpdateWatched(string id, bool isWatched);
	}
}