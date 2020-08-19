using Common.Domain;

namespace MovOrg.Organizer.Domain
{
	//TODO: this must be merged with common images
	//TODO: just realized here is the database performance issue
	public class MovieImageData : Entity
	{
		public Movie Movie { get; set; }
		public string MovieId { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }
	}
}