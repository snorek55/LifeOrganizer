using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Rating : Entity, IEquatable<Rating>
	{
		public float? Score { get; set; }

		public string MovieId { get; set; }
		public Movie Movie { get; set; }

		public string SourceId { get; set; }
		public RatingSource Source { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Rating);
		}

		public bool Equals(Rating other)
		{
			return other != null &&
				   Score == other.Score &&
				   Movie.Id == other.Movie.Id &&
				   SourceId == other.SourceId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Score, Movie.Id, SourceId);
		}
	}
}