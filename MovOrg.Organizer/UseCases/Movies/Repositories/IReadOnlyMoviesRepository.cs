using MovOrg.Organizer.UseCases.DTOs;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface IReadOnlyMoviesRepository
	{
		Task<MovieWithDetailsDto> GetMovieDetailsById(string id);
	}
}