using Common.Domain;

namespace MovOrg.Organizer.Domain
{
	public class MovieImageData : Entity
	{
		public Movie Movie { get; set; }
		public string MovieId { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }
	}
}