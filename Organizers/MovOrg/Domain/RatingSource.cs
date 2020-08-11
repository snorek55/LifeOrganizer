using Common.Domain;

using System.Collections.Generic;

namespace Organizers.MovOrg.Domain
{
	public class RatingSource : Entity
	{
		public string Name { get; set; }

		public List<Rating> Ratings { get; set; }

		public string LogoUrl { get; set; }
	}
}