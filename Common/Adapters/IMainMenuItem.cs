using Organizers.Main.Adapters.MainMenu;

namespace Common.Adapters
{
	public interface IMainMenuItem
	{
		string Label { get; }
		MainMenuIconType Icon { get; }
		BaseOrganizerContainerViewModel OrganizerContainer { get; }
	}
}