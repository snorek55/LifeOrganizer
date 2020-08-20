using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface IApiMoviesRepository
	{
		Task<IEnumerable<Movie>> GetTopMovies();

		Task<IEnumerable<Movie>> GetMoviesFromSuggestedTitle(string suggestedTitle);

		Task<UpdateMovieDetailsDto> GetAllMovieDetailsById(string id);
	}
}