namespace MovOrg.Organizer.UseCases.Requests
{
	public class GetMovieImagesRequest
	{
		public string MovieId { get; }

		public GetMovieImagesRequest(string movieId)
		{
			MovieId = movieId;
		}
	}
}