using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Actor : Entity, IEquatable<Actor>
	{
		public string ImageUrl { get; set; }
		public string Name { get; set; }
		public string AsCharacter { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Actor);
		}

		public bool Equals(Actor other)
		{
			return other != null &&
				   Id == other.Id &&
				   ImageUrl == other.ImageUrl &&
				   Name == other.Name &&
				   AsCharacter == other.AsCharacter;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, ImageUrl, Name, AsCharacter);
		}
	}
}