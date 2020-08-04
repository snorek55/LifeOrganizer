using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class MovieImageData : Entity, IEquatable<MovieImageData>
	{
		public Movie Movie { get; set; }
		public string MovieId { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieImageData);
		}

		public bool Equals(MovieImageData other)
		{
			return other != null &&
				   Id == other.Id &&
				   Image == other.Image &&
				   Title == other.Title;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Image, Title);
		}
	}
}