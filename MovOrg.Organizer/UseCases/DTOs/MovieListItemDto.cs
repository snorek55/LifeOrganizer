namespace MovOrg.Organizer.UseCases.DTOs
{
	public class MovieListItemDto
	{
		#region Basic Data

		public string Id { get; set; }

		public string Title { get; set; }
		public float? IMDbRating { get; set; }
		public int? Rank { get; set; }
		public string CoverImageUrl { get; set; }
		public string Year { get; set; }

		#endregion Basic Data
	}
}