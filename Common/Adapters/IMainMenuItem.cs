using Organizers.Main.Adapters.MainMenu;

namespace Common.Adapters
{
	public interface IMainMenuItem
	{
		string Label { get; set; }
		MainMenuIconType Icon { get; set; }
		BaseOrganizerContainerViewModel OrganizerContainer { get; set; }
	}
}