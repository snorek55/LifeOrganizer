using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieCompany
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string CompanyId { get; set; }

		public Company Company { get; set; }
	}
}