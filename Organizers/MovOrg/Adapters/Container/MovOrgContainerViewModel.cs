using Organizers.Main.Adapters;
using Organizers.Main.Adapters.Sections;

using System.Collections.ObjectModel;

namespace Organizers.MovOrg.Adapters.Container
{
	public class MovOrgContainerViewModel : OrganizerContainerViewModel
	{
		public MovOrgContainerViewModel(ISectionsFactory sectionsFactory) : base(sectionsFactory)
		{
			Sections = new ObservableCollection<SectionViewModel>(sectionsFactory.GenerateMoviesSections());

			SelectedSection = Sections[0];
		}
	}
}