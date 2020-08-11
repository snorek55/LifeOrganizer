using MovOrg.Organizers.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizers.UseCases.Repositories
{
	public interface IApiMoviesRepository : IReadOnlyMoviesRepository
	{
		Task<IEnumerable<Movie>> GetTopMovies();

		Task<IEnumerable<Movie>> GetMoviesFromSuggestedTitle(string suggestedTitle);
	}
}