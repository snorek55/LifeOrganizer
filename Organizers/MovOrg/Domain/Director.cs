using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Director : Entity, IEquatable<Director>
	{
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Director);
		}

		public bool Equals(Director other)
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