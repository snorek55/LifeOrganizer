using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieWriter : IEquatable<MovieWriter>
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string WriterId { get; set; }

		public Writer Writer { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieWriter);
		}

		public bool Equals(MovieWriter other)
		{
			return other != null &&
				   MovieId == other.MovieId &&
				   WriterId == other.WriterId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(MovieId, WriterId);
		}
	}
}