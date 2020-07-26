using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class GetMovieDetailsResponse : ResponseBase
	{
		public Movie Movie { get; set; }

		public GetMovieDetailsResponse()
		{
		}

		public GetMovieDetailsResponse(Movie movie) : base()
		{
			Movie = movie;
		}

		public GetMovieDetailsResponse(string error) : base(error)
		{
		}
	}
}