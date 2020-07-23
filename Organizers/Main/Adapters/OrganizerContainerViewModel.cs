using System.Collections.ObjectModel;

namespace Organizers.Main.Adapters
{
	public class OrganizerContainerViewModel
	{
		public ObservableCollection<SectionViewModel> Sections { get; set; } = new ObservableCollection<SectionViewModel>();
		public SectionViewModel SelectedSection { get; set; }
	}
}