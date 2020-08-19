using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class RatingDto
	{
		public string SourceName { get; set; }
		public string SourceLogoUrl { get; set; }
		public string Score { get; set; }
		public string MovieId { get; set; }
		public string SourceId { get; set; }
		public RatingSource Source { get; set; }
		public Movie Movie { get; set; }
	}
}