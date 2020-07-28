using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieActor : IEquatable<MovieActor>
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string ActorId { get; set; }

		public Actor Actor { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieActor);
		}

		public bool Equals(MovieActor other)
		{
			return other != null &&
				   MovieId == other.MovieId &&
				   ActorId == other.ActorId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(MovieId, ActorId);
		}
	}
}