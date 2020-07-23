using Organizers.Common.UseCases;

namespace Organizers.MovOrg.UseCases.Responses
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