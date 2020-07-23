using Domain;

using Organizers.Common.UseCases;

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