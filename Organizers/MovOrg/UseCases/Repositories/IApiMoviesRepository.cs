using Organizers.MovOrg.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizers.MovOrg.UseCases.Repositories
{
	public interface IApiMoviesRepository : IReadOnlyMoviesRepository
	{
		Task<IEnumerable<Movie>> GetTopMovies();

		Task<IEnumerable<Movie>> GetMoviesFromSuggestedTitle(string suggestedTitle);
	}
}