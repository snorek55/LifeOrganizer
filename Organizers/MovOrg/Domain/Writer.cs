using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Writer : Entity, IEquatable<Writer>
	{
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Writer);
		}

		public bool Equals(Writer other)
		{
			return other != null &&
				   Id == other.Id &&
				   Name == other.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Name);
		}
	}
}