using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class Company : Entity, IEquatable<Company>
	{
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as Company);
		}

		public bool Equals(Company other)
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