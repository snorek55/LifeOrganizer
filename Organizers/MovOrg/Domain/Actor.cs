using Organizers.Common.Domain;

namespace Domain
{
	public class Actor : Entity
	{
		public string ImageUrl { get; set; }
		public string Name { get; set; }
		public string AsCharacter { get; set; }
	}
}