using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class MovieImageDto
	{
		public string Id { get; set; }
		public Movie Movie { get; set; }
		public string MovieId { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }
	}
}