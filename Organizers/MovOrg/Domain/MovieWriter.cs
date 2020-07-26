namespace Organizers.MovOrg.Domain
{
	public class MovieWriter
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string WriterId { get; set; }

		public Writer Writer { get; set; }
	}
}