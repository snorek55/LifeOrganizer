using Organizers.Common.Adapters;

namespace Organizers.Main.Adapters
{
	public class MainMenuItemViewModel : BaseViewModel
	{
		public string Label { get; set; }
		public MainMenuIconType Icon { get; set; }
		public OrganizerContainerViewModel OrganizerContainer { get; set; }

		public MainMenuItemViewModel(string label, MainMenuIconType icon, OrganizerContainerViewModel organizer)
		{
			Label = label;
			Icon = icon;
			OrganizerContainer = organizer;
		}
	}
}