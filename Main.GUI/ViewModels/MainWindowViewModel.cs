using Common.Adapters;

using System.Collections.ObjectModel;

namespace Main.GUI.ViewModels
{
	public class MainWindowViewModel : BaseViewModel
	{
		public ObservableCollection<IContainerPluginData> MainMenuContainers { get; } = new ObservableCollection<IContainerPluginData>();
		public ObservableCollection<IContainerPluginData> MainMenuOptionsContainers { get; } = new ObservableCollection<IContainerPluginData>();

		public IContainerPluginData SelectedItem { get; set; }
		public BaseOrganizerContainerViewModel CurrentOrganizerContainer { get; set; }

		public void AddMainMenuItem(IContainerPluginData mainMenuItem)
		{
			MainMenuContainers.Add(mainMenuItem);
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