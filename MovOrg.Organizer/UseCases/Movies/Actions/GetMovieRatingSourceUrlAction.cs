using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Organizer.UseCases.Requests;

using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMovieRatingSourceUrlAction : ServiceActionBase, IServiceAction<GetMovieRatingSourceUrlRequest, MovieRatingSourceDto>
	{
		private IMovieDetailsDbAccess dbAccess;
		private IApiMoviesRepository apiAccess;

		public GetMovieRatingSourceUrlAction(IMovieDetailsDbAccess dbAccess, IApiMoviesRepository apiAccess)
		{
			this.dbAccess = dbAccess;
			this.apiAccess = apiAccess;
		}

		public async Task<MovieRatingSourceDto> Action(GetMovieRatingSourceUrlRequest request)
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

			MovieRatingSourceDto source = null;
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