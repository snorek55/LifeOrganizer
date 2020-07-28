using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Trailer : Entity, IEquatable<Trailer>
	{
		public Movie Movie { get; set; }
		public string ThumbnailUrl { get; set; }
		public string LinkEmbed { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Trailer);
		}

		public bool Equals(Trailer other)
		{
			return other != null &&
				   Id == other.Id &&
				   Movie.Id == other.Movie.Id &&
				   ThumbnailUrl == other.ThumbnailUrl &&
				   LinkEmbed == other.LinkEmbed;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Movie.Id, ThumbnailUrl, LinkEmbed);
		}
	}
}