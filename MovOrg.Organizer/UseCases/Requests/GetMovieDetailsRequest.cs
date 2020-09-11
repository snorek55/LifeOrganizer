namespace MovOrg.Organizer.UseCases.Requests
{
	public class GetMovieDetailsRequest
	{
		public string MovieId { get; }

		public GetMovieDetailsRequest(string movieId)
		{
			MovieId = movieId;
		}
	}
}