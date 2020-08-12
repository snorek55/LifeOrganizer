using MovOrg.Organizers.Domain;

using System.Threading.Tasks;

namespace MovOrg.Organizers.UseCases.Repositories
{
	public interface IReadOnlyMoviesRepository
	{
		Task<Movie> GetMovieDetailsById(string id);
	}
}