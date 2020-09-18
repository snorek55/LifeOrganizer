using Common.UseCases;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class UpdateFavoriteResponse : ResponseBase
	{
		public UpdateFavoriteResponse() : base(null)
		{
		}

		public UpdateFavoriteResponse(string error) : base(error)
		{
		}
	}
}