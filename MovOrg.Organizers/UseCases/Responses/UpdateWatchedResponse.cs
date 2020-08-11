using Common.UseCases;

namespace MovOrg.Organizers.UseCases.Responses
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