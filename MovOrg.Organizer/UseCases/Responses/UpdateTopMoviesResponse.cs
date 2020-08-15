using Common.UseCases;

namespace MovOrg.Organizer.UseCases.Responses
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