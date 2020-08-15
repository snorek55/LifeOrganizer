using MovOrg.Organizer.Domain;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface IReadOnlyMoviesRepository
	{
		Task<Movie> GetMovieDetailsById(string id);
	}
}