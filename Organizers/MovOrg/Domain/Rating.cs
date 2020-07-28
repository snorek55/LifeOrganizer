using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Rating : Entity, IEquatable<Rating>
	{
		public float? Score { get; set; }
		public Movie Movie { get; set; }

		public int RatingSourceId { get; set; }
		public RatingSource RatingSource { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Rating);
		}

		public bool Equals(Rating other)
		{
			return other != null &&
				   Id == other.Id &&
				   Score == other.Score &&
				   Movie.Id == other.Movie.Id &&
				   RatingSourceId == other.RatingSourceId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Score, Movie.Id, RatingSourceId);
		}
	}
}