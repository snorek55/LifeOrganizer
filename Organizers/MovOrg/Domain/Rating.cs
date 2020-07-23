using Organizers.Common.Domain;

namespace Domain
{
	public class Rating : Entity
	{
		public float? Score { get; set; }
		public Movie Movie { get; set; }

		public int RatingSourceId { get; set; }
		public RatingSource RatingSource { get; set; }
	}
}