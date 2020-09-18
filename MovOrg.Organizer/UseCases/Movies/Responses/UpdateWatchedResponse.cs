using Common.UseCases;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class UpdateWatchedResponse : ResponseBase
	{
		public UpdateWatchedResponse() : base(null)
		{
		}

		public UpdateWatchedResponse(string error) : base(error)
		{
		}
	}
}