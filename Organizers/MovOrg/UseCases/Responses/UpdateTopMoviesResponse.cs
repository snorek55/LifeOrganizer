using Organizers.Common.UseCases;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class UpdateTopMoviesResponse : ResponseBase
	{
		//TODO: avoid this by using constructor default in base
		public UpdateTopMoviesResponse() : base(null)
		{
		}

		public UpdateTopMoviesResponse(string error) : base(error)
		{
		}
	}
}