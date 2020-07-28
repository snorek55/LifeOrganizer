using Organizers.Common.Domain;

using System;

namespace Organizers.MovOrg.Domain
{
	public class ImageData : Entity, IEquatable<ImageData>
	{
		public string Image { get; set; }
		public string Title { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as ImageData);
		}

		public bool Equals(ImageData other)
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