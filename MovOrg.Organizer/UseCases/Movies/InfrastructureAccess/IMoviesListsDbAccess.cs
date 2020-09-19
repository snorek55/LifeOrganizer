using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMoviesListsDbAccess
	{
		Task<IEnumerable<MovieListItemDto>> GetMoviesFromLocal();

		Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle);

		Task UpdateSuggestedTitleMovies(IEnumerable<MovieListItemDto> movies);
	}
}