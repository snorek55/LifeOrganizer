using Common.UseCases;

namespace MovOrg.Organizer.UseCases.Responses
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