using Organizers.Common.Domain;

namespace Organizers.MovOrg.Domain
{
	public class Actor : Entity
	{
		public string ImageUrl { get; set; }
		public string Name { get; set; }
		public string AsCharacter { get; set; }
	}
}