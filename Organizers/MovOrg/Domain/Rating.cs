using Common.Domain;

namespace Organizers.MovOrg.Domain
{
	public class Rating : Entity
	{
		public float? Score { get; set; }

		public string MovieId { get; set; }
		public Movie Movie { get; set; }

		public string SourceId { get; set; }
		public RatingSource Source { get; set; }
	}
}