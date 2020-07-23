using Organizers.Common.Adapters;

using System.Collections.ObjectModel;
using System.Linq;

namespace Organizers.Main.Adapters
{
	public class MainWindowViewModel : BaseViewModel
	{
		public ObservableCollection<MainMenuItemViewModel> MainMenuItems { get; } = new ObservableCollection<MainMenuItemViewModel>();
		public ObservableCollection<MainMenuItemViewModel> MainMenuOptionsItems { get; } = new ObservableCollection<MainMenuItemViewModel>();

		public MainMenuItemViewModel SelectedItem { get; set; }
		public OrganizerContainerViewModel CurrentOrganizerContainer { get; set; }

		public MainWindowViewModel(IMainMenuItemFactory mainMenuItemFactory)
		{
			MainMenuOptionsItems.Add(mainMenuItemFactory.GenerateAboutMenu());
			MainMenuOptionsItems.Add(mainMenuItemFactory.GenerateConfigMenu());
			MainMenuItems.Add(mainMenuItemFactory.GenerateFoodMenu());
			MainMenuItems.Add(mainMenuItemFactory.GenerateGamesMenu());

			MainMenuItems.Add(mainMenuItemFactory.GenerateGymMenu());
			MainMenuItems.Add(mainMenuItemFactory.GenerateMoviesMenu());

			SelectedItem = MainMenuItems.Last();
		}

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados

		private void OnSelectedItemChanged()
#pragma warning restore IDE0051 // Quitar miembros privados no utilizados
		{
			if (SelectedItem == null)
				return;

			if (CurrentOrganizerContainer != SelectedItem.OrganizerContainer)
			{
				CurrentOrganizerContainer = SelectedItem.OrganizerContainer;
			}
		}
	}
}