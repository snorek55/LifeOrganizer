using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.Requests;

using System;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public class UpdateUserMovieStatusAction : ServiceActionBase, IServiceAction<UpdateUserMovieStatusRequest>
	{
		private IMovieDetailsDbAccess dbAccess;

		public UpdateUserMovieStatusAction(IMovieDetailsDbAccess dbAccess)
		{
			this.dbAccess = dbAccess;
		}

		public async Task<bool> Action(UpdateUserMovieStatusRequest request)
		{
			if (request?.MovieId == null)
			{
				AddError("Null id or request");
				return false;
			}

			switch (request.MovieUserStatus)
			{
				case MovieUserStatus.MustWatch:
					dbAccess.MarkMovieAsMustWatch(request.MovieId, request.Marked);
					break;

				case MovieUserStatus.IsFavorite:
					dbAccess.MarkMovieAsFavorite(request.MovieId, request.Marked);
					break;

				case MovieUserStatus.Watched:
					dbAccess.MarkMovieAsWatched(request.MovieId, request.Marked);
					break;

				default:
					throw new NotImplementedException();
			}

			return !HasErrors;
		}
	}
}