using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Requests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMoviesFromLocalAction : ServiceActionBase, IServiceAction<GetMoviesFromLocalRequest, IEnumerable<MovieListItemDto>>
	{
		private IMoviesFromLocalDbAccess dbAccess;

		public GetMoviesFromLocalAction(IMoviesFromLocalDbAccess dbAccess)
		{
			this.dbAccess = dbAccess;
		}

		public async Task<IEnumerable<MovieListItemDto>> Action(GetMoviesFromLocalRequest request)
		{
			var moviesFromLocal = await dbAccess.GetMoviesFromLocal();

			if (moviesFromLocal == null || moviesFromLocal.Count() == 0)
				AddError($"No movies found.");

			return HasErrors ? null : moviesFromLocal;
		}
	}
}