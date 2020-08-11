using Common.UseCases;

namespace Organizers.MovOrg.UseCases.Responses
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