using Organizers.MovOrg.Domain;

using System.Threading.Tasks;

namespace Organizers.MovOrg.UseCases.Repositories
{
	public interface IReadOnlyMoviesRepository
	{
		Task<Movie> GetMovieDetailsById(string id);

		Task<Person> GetPersonDetails(string id);
	}
}