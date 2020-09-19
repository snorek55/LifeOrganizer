namespace MovOrg.Organizer.UseCases.Movies.Requests
{
	public class GetMoviesFromSuggestedTitleRequest
	{
		public string SuggestedTitle { get; }

		public GetMoviesFromSuggestedTitleRequest(string suggestedTitle)
		{
			SuggestedTitle = suggestedTitle;
		}
	}
}