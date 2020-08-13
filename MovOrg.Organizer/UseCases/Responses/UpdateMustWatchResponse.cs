using Common.UseCases;

namespace MovOrg.Organizers.UseCases.Responses
{
	public class UpdateMustWatchResponse : ResponseBase
	{
		public UpdateMustWatchResponse() : base(null)
		{
		}

		public UpdateMustWatchResponse(string error) : base(error)
		{
		}
	}
}