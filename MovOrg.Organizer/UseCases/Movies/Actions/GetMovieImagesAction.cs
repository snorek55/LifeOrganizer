using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Requests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMovieImagesAction : ServiceActionBase, IServiceAction<GetMovieImagesRequest, IEnumerable<MovieImageDto>>
	{
		private IMovieDetailsDbAccess dbAccess;

		public GetMovieImagesAction(IMovieDetailsDbAccess dbAccess)
		{
			this.dbAccess = dbAccess;
		}

		public async Task<IEnumerable<MovieImageDto>> Action(GetMovieImagesRequest request)
		{
			if (request?.MovieId == null)
			{
				AddError("Null id or request");
				return null;
			}
			var movieImages = await dbAccess.GetMovieImages(request.MovieId);

			if (movieImages == null || movieImages.Count() == 0)
				AddError($"Movie images not found.");

			return HasErrors ? null : movieImages;
		}
	}
}