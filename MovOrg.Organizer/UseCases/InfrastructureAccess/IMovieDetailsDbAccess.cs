using MovOrg.Organizer.UseCases.DTOs;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMovieDetailsDbAccess
	{
		Task<MovieWithDetailsDto> GetMovieDetails(string id);

		Task AddMovieDetails(UpdateMovieDetailsDto movieDetails);

		Task<bool> AreDetailsAvailableFor(string movieId);
	}
}