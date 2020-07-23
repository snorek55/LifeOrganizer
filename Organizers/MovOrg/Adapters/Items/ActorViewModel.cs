using Organizers.Common.Adapters;

namespace Organizers.MovOrg.Adapters.Items
{
	public class ActorViewModel : BaseViewModel
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string AsCharacter { get; set; }

		public string ImageUrl
		{
			get;
			set;
		}
	}
}