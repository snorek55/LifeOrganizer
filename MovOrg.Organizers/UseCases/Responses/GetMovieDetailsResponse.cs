using Common.UseCases;

using MovOrg.Organizers.Domain;

namespace MovOrg.Organizers.UseCases.Responses
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