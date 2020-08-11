using System;

namespace Common.Domain
{
	public abstract class Entity : IEquatable<Entity>
	{
		public string Id { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Entity);
		}

		public bool Equals(Entity other)
		{
			return other != null &&
				   Id == other.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}