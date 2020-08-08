namespace Organizers.MovOrg.Domain
{
	public class MovieSimilar
	{
		public string SimilarId { get; set; }
		public Movie Similar { get; set; }

		public Movie Movie { get; set; }
		public string MovieId { get; set; }
	}
}