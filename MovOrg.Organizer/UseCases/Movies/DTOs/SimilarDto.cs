using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class SimilarDto
	{
		public string CoverImageUrl { get; set; }
		public object SimilarId { get; set; }
		public Movie Similar { get; set; }
		public object MovieId { get; set; }
	}
}