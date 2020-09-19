using Common.Setup;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Movies.Requests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMoviesFromSuggestedTitleAction : ServiceActionBase, IServiceAction<GetMoviesFromSuggestedTitleRequest, IEnumerable<MovieListItemDto>>
	{
		private readonly IMoviesListsDbAccess dbAccess;
		private readonly IMoviesListsApiAccess apiAccess;
		private readonly IConfig config;

		public GetMoviesFromSuggestedTitleAction(IMoviesListsDbAccess dbAccess, IMoviesListsApiAccess apiAccess, IConfig config)
		{
			this.dbAccess = dbAccess;
			this.apiAccess = apiAccess;
			this.config = config;
		}

		public async Task<IEnumerable<MovieListItemDto>> Action(GetMoviesFromSuggestedTitleRequest request)
		{
			var wasSearched = await config.WasAlreadySearched(request.SuggestedTitle);
			IEnumerable<MovieListItemDto> moviesFromSuggestedTitle;
			if (!wasSearched)
			{
				moviesFromSuggestedTitle = await apiAccess.GetMoviesFromSuggestedTitle(request.SuggestedTitle);
				await dbAccess.UpdateSuggestedTitleMovies(moviesFromSuggestedTitle);
				await config.AddSearchedTitleAsync(request.SuggestedTitle);
			}
			else
			{
				moviesFromSuggestedTitle = await dbAccess.GetMoviesFromSuggestedTitle(request.SuggestedTitle);
			}

			if (moviesFromSuggestedTitle == null || moviesFromSuggestedTitle.Count() == 0)
				AddError($"No movies found.");

			return HasErrors ? null : moviesFromSuggestedTitle;
		}
	}
}