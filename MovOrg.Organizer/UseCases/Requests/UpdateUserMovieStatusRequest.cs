namespace MovOrg.Organizer.UseCases.Requests
{
	public enum MovieUserStatus
	{
		MustWatch,
		IsFavorite,
		Watched
	}

	public class UpdateUserMovieStatusRequest
	{
		public MovieUserStatus MovieUserStatus { get; }
		public string MovieId { get; }
		public bool Marked { get; }

		public UpdateUserMovieStatusRequest(MovieUserStatus movieUserStatus, string movieId, bool marked)
		{
			MovieUserStatus = movieUserStatus;
			MovieId = movieId;
			Marked = marked;
		}
	}
}