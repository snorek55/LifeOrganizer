using Organizers.Common.Domain;

using System.Collections.Generic;

namespace Domain
{
	public class RatingSource : Entity
	{
		public string Name { get; set; }

		public List<Rating> Ratings { get; set; }
	}
}