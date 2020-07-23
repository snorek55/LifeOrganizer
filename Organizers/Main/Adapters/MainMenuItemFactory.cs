using Organizers.GamesOrg;
using Organizers.MovOrg.Adapters.Container;

namespace Organizers.Main.Adapters
{
	public class MainMenuItemFactory : IMainMenuItemFactory
	{
		private MovOrgContainerViewModel movOrgContainer;

		public MainMenuItemFactory(MovOrgContainerViewModel movOrgContainer)
		{
			this.movOrgContainer = movOrgContainer;
		}

		public MainMenuItemViewModel GenerateAboutMenu()
		{
			return new MainMenuItemViewModel("About", MainMenuIconType.About, null);
		}

		public MainMenuItemViewModel GenerateConfigMenu()
		{
			return new MainMenuItemViewModel("Configuration", MainMenuIconType.Options, null);
		}

		public MainMenuItemViewModel GenerateFoodMenu()
		{
			return new MainMenuItemViewModel("Food", MainMenuIconType.Diet, null);
		}

		public MainMenuItemViewModel GenerateGamesMenu()
		{
			return new MainMenuItemViewModel("Games", MainMenuIconType.Games, new GamesOrgContainerViewModel());
		}

		public MainMenuItemViewModel GenerateGymMenu()
		{
			return new MainMenuItemViewModel("Gym", MainMenuIconType.Gym, null);
		}

		public MainMenuItemViewModel GenerateMoviesMenu()
		{
			return new MainMenuItemViewModel("Movies", MainMenuIconType.Movies, movOrgContainer);
		}
	}
}