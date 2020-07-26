namespace Organizers.Main.Adapters.MainMenu
{
	public interface IMainMenuItemFactory
	{
		MainMenuItemViewModel GenerateMoviesMenu();

		MainMenuItemViewModel GenerateGamesMenu();

		MainMenuItemViewModel GenerateFoodMenu();

		MainMenuItemViewModel GenerateGymMenu();

		MainMenuItemViewModel GenerateConfigMenu();

		MainMenuItemViewModel GenerateAboutMenu();
	}
}