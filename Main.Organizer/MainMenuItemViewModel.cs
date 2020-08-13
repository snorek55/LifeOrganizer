using Common.Adapters;

using Organizers.Main.Adapters.MainMenu;

namespace Organizers
{
	public class MainMenuItemViewModel : BaseViewModel, IMainMenuItem
	{
		public string Label { get; set; }
		public MainMenuIconType Icon { get; set; }
		public BaseOrganizerContainerViewModel OrganizerContainer { get; set; }

		public MainMenuItemViewModel(string label, MainMenuIconType icon, BaseOrganizerContainerViewModel organizer)
		{
			Label = label;
			Icon = icon;
			OrganizerContainer = organizer;
		}
	}
}