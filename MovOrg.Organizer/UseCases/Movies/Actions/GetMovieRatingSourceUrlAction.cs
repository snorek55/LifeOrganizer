using Common.UseCases;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Requests;

using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMovieRatingSourceUrlAction : ServiceActionBase, IServiceActionAsync<GetMovieRatingSourceUrlRequest, MovieRatingSourceUrlDto>
	{
		private IMovieDetailsDbAccess dbAccess;
		private IMoviesListsApiAccess apiAccess;

		public GetMovieRatingSourceUrlAction(IMovieDetailsDbAccess dbAccess, IMoviesListsApiAccess apiAccess)
		{
			this.dbAccess = dbAccess;
			this.apiAccess = apiAccess;
		}

		public async Task<MovieRatingSourceUrlDto> Action(GetMovieRatingSourceUrlRequest request)
		{
			if (request?.MovieId == null)
			{
				AddError("Null id or request");
				return null;
			}
			var sourcesWebPages = dbAccess.GetRatingSourceUrls(request.MovieId);
			//TODO: create a boolean to check that all rating sources urls have been loaded
			var nullWebPages = sourcesWebPages.Where(x => x.SourceUrl == null);
			if (nullWebPages.Count() == sourcesWebPages.Count())
			{
				sourcesWebPages = await apiAccess.GetRatingSourcesUrls(request.MovieId);
				dbAccess.UpdateSourcesWebPages(request.MovieId, sourcesWebPages);
			}

			MovieRatingSourceUrlDto source = null;
			foreach (var s in sourcesWebPages)
			{
				if (s.SourceName == request.SourceName)
					source = s;
			}

			if (source == null)
				AddError($"Rating source not found.");

			return HasErrors ? null : source;
		}
	}
}