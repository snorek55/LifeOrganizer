using MovOrg.Organizer.UseCases.DTOs;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMovieDetailsApiAccess
	{
		Task<UpdateMovieDetailsDto> GetMovieDetails(string id);
	}
}