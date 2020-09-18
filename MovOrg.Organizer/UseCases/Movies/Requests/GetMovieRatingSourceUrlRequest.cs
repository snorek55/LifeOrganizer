namespace MovOrg.Organizer.UseCases.Requests
{
	public class GetMovieRatingSourceUrlRequest
	{
		public string MovieId { get; }
		public string SourceName { get; }

		public GetMovieRatingSourceUrlRequest(string movieId, string sourceName)
		{
			MovieId = movieId;
			SourceName = sourceName;
		}
	}
}