using Common.UseCases;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class UpdateTopMoviesResponse : ResponseBase
	{
		public UpdateTopMoviesResponse()
		{
		}

		public UpdateTopMoviesResponse(string error) : base(error)
		{
		}
	}
}