using Common;
using Common.Setup;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.APIs
{
	public class ImdbMoviesListsApiAccess : BaseImdbApiAccess, IMoviesListsApiAccess
	{
		public ImdbMoviesListsApiAccess(IAutoMapper mapper, IConfig config) : base(mapper, config)
		{
		}

		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle)
		{
			Ensure.IsNotNull(suggestedTitle);
			var data = await apiLib.SearchMovieAsync(suggestedTitle);
			ThrowIfError(data.ErrorMessage);
			return mapper.Map<IEnumerable<MovieListItemDto>>(data.Results);
		}

		public async Task<IEnumerable<MovieListItemDto>> GetTopMovies()
		{
			var data = await apiLib.Top250MoviesAsync();
			ThrowIfError(data.ErrorMessage);
			var movies = mapper.Map<IEnumerable<MovieListItemDto>>(data.Items);
			return movies;
		}

		public async Task<IEnumerable<MovieRatingSourceUrlDto>> GetRatingSourcesUrls(string movieId)
		{
			var data = await apiLib.ExternalSitesAsync(movieId);

			var ratingSourceUrl = mapper.Map<IEnumerable<MovieRatingSourceUrlDto>>(data);
			return ratingSourceUrl;
		}
	}
}