using Common.Adapters;

using Organizers.Main.Adapters.MainMenu;

namespace MovOrg.Organizers.Adapters.Container
{
	public class MovOrgMainMenuItemViewModel : IMainMenuItem
	{
		public string Label => "Movies";
		public MainMenuIconType Icon => MainMenuIconType.Movies;
		public BaseOrganizerContainerViewModel OrganizerContainer { get; private set; }

		public MovOrgMainMenuItemViewModel(MovOrgContainerViewModel movOrgContainerViewModel)
		{
			OrganizerContainer = movOrgContainerViewModel;
		}
	}
}