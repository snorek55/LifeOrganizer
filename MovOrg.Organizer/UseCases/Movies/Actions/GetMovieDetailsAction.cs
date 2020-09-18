using Common.Setup;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Requests;

using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class GetMovieDetailsAction : ServiceActionBase, IServiceAction<GetMovieDetailsRequest, MovieWithDetailsDto>
	{
		private IMovieDetailsDbAccess dbAccess;
		private IMovieDetailsApiAccess apiAccess;
		private IAutoMapper mapper;

		public GetMovieDetailsAction(IMovieDetailsDbAccess dbAccess, IMovieDetailsApiAccess apiAccess, IAutoMapper mapper)
		{
			this.dbAccess = dbAccess;
			this.apiAccess = apiAccess;
			this.mapper = mapper;
		}

		public async Task<MovieWithDetailsDto> Action(GetMovieDetailsRequest request)
		{
			if (request?.MovieId == null)
			{
				AddError("Null id or request");
				return null;
			}

			var areDetailsAvailable = await dbAccess.AreDetailsAvailableFor(request.MovieId);

			MovieWithDetailsDto movieWithDetails = null;
			if (!areDetailsAvailable)
			{
				var movieDetailsFromApi = await apiAccess.GetMovieDetails(request.MovieId);
				await dbAccess.AddMovieDetails(movieDetailsFromApi);
				//It does not get the info from the db because we do not save changes so I do it like this.
				movieWithDetails = mapper.Map<MovieWithDetailsDto>(movieDetailsFromApi);
			}

			if (movieWithDetails == null)
				movieWithDetails = await dbAccess.GetMovieDetails(request.MovieId);

			if (movieWithDetails == null)
				AddError($"Movie {request.MovieId} not found.");

			return HasErrors ? null : movieWithDetails;
		}
	}
}