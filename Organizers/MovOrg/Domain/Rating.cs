using Organizers.Common.Domain;

namespace Organizers.MovOrg.Domain
{
	public class Rating : Entity
	{
		public float? Score { get; set; }
		public Movie Movie { get; set; }

		public int RatingSourceId { get; set; }
		public RatingSource RatingSource { get; set; }
	}
}