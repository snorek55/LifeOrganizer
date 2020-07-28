using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class BoxOffice : Entity, IEquatable<BoxOffice>
	{
		public Movie Movie { get; set; }

		public string Budget { get; set; }
		public string OpeningWeekendUSA { get; set; }
		public string GrossUSA { get; set; }
		public string CumulativeWorldwideGross { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as BoxOffice);
		}

		public bool Equals(BoxOffice other)
		{
			return other != null &&
				   Id == other.Id &&
				   Movie.Id == other.Movie.Id &&
				   Budget == other.Budget &&
				   OpeningWeekendUSA == other.OpeningWeekendUSA &&
				   GrossUSA == other.GrossUSA &&
				   CumulativeWorldwideGross == other.CumulativeWorldwideGross;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Movie.Id, Budget, OpeningWeekendUSA, GrossUSA, CumulativeWorldwideGross);
		}
	}
}