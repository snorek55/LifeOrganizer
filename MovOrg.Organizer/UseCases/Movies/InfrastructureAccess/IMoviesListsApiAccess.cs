﻿using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMoviesListsApiAccess
	{
		Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle);

		Task<IEnumerable<MovieListItemDto>> GetTopMovies();
		Task<IEnumerable<MovieRatingSourceUrlDto>> GetRatingSourcesUrls(string movieId);
	}
}