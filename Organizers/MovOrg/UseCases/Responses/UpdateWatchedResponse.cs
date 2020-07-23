using Organizers.Common.UseCases;

namespace Organizers.MovOrg.UseCases.Responses
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