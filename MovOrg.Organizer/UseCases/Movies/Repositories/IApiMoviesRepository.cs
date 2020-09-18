using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface IApiMoviesRepository
	{
		Task<IEnumerable<MovieListItemDto>> GetTopMovies();

		Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle);

		Task<UpdateMovieDetailsDto> GetAllMovieDetailsById(string id);

		Task<IEnumerable<MovieRatingSourceDto>> GetRatingSourcesUrls(string id);
	}
}