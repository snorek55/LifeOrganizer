using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieCompany : IEquatable<MovieCompany>
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string CompanyId { get; set; }

		public Company Company { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieCompany);
		}

		public bool Equals(MovieCompany other)
		{
			return other != null &&
				   MovieId == other.MovieId &&
				   CompanyId == other.CompanyId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(MovieId, CompanyId);
		}
	}
}