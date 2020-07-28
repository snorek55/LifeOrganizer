using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieDirector : IEquatable<MovieDirector>
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string DirectorId { get; set; }

		public Director Director { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieDirector);
		}

		public bool Equals(MovieDirector other)
		{
			return other != null &&
				   MovieId == other.MovieId &&
				   DirectorId == other.DirectorId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(MovieId, DirectorId);
		}
	}
}