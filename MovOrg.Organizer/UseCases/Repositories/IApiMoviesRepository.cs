using MovOrg.Organizer.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface IApiMoviesRepository : IReadOnlyMoviesRepository
	{
		Task<IEnumerable<Movie>> GetTopMovies();

		Task<IEnumerable<Movie>> GetMoviesFromSuggestedTitle(string suggestedTitle);
	}
}