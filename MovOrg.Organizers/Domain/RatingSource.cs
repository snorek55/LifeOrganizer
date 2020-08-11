using Common.Domain;

using System.Collections.Generic;

namespace MovOrg.Organizers.Domain
{
	public class RatingSource : Entity
	{
		public string Name { get; set; }

		public List<Rating> Ratings { get; set; }

		public string LogoUrl { get; set; }
	}
}