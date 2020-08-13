using Common.Adapters;

using System.Collections.ObjectModel;

namespace Organizers
{
	public class MainWindowViewModel : BaseViewModel
	{
		public ObservableCollection<IMainMenuItem> MainMenuItems { get; } = new ObservableCollection<IMainMenuItem>();
		public ObservableCollection<IMainMenuItem> MainMenuOptionsItems { get; } = new ObservableCollection<IMainMenuItem>();

		public IMainMenuItem SelectedItem { get; set; }
		public BaseOrganizerContainerViewModel CurrentOrganizerContainer { get; set; }

		public void AddMainMenuItem(IMainMenuItem mainMenuItem)
		{
			MainMenuItems.Add(mainMenuItem);
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