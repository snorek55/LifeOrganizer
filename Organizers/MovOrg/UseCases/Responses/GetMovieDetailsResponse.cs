using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class GetMovieDetailsResponse : ResponseBase
	{
		public Movie Movie { get; set; }

		public GetMovieDetailsResponse(Movie movie) : base(null)
		{
			Movie = movie;
		}

		public GetMovieDetailsResponse(string error) : base(error)
		{
		}
	}
}