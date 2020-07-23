namespace Organizers.Main.Adapters
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