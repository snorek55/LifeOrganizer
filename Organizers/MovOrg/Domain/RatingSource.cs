using Organizers.Common.Domain;

using System;
using System.Collections.Generic;

namespace Organizers.MovOrg.Domain
{
	public class RatingSource : Entity, IEquatable<RatingSource>
	{
		public string Name { get; set; }

		public List<Rating> Ratings { get; set; }

		public string LogoUrl { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as RatingSource);
		}

		public bool Equals(RatingSource other)
		{
			return other != null &&
				   Id == other.Id &&
				   LogoUrl == other.LogoUrl &&
				   Name == other.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Name, LogoUrl);
		}
	}
}