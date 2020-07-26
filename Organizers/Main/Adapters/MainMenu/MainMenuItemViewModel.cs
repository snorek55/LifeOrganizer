using Organizers.Common.Adapters;
using Organizers.Main.Adapters.Sections;

namespace Organizers.Main.Adapters.MainMenu
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