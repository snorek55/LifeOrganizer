using Common.UseCases;

namespace MovOrg.Organizers.UseCases.Responses
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