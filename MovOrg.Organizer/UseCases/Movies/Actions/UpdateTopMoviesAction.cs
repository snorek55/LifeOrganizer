using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.Requests;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class UpdateTopMoviesAction : ServiceActionBase, IServiceAction<UpdateTopMoviesRequest>
	{
		private IMoviesListsDbAccess dbAccess;
		private IMoviesListsApiAccess apiAccess;

		public UpdateTopMoviesAction(IMoviesListsDbAccess dbAccess, IMoviesListsApiAccess apiAccess)
		{
			this.dbAccess = dbAccess;
			this.apiAccess = apiAccess;
		}

		public async Task<bool> Action(UpdateTopMoviesRequest request)
		{
			var topApiMovies = await apiAccess.GetTopMovies();
			await dbAccess.UpdateTopMovies(topApiMovies);
			return !HasErrors;
		}
	}
}