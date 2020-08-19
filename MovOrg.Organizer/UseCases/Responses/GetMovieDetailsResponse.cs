using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetMovieDetailsResponse : ResponseBase
	{
		public MovieWithDetailsDto Movie { get; set; }

		public GetMovieDetailsResponse()
		{
		}

		public GetMovieDetailsResponse(MovieWithDetailsDto movie) : base()
		{
			Movie = movie;
		}

		public GetMovieDetailsResponse(string error) : base(error)
		{
		}
	}
}