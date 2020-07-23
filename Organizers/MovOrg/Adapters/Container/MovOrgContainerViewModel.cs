using Organizers.Main.Adapters;
using Organizers.MovOrg.Adapters.Sections;

namespace Organizers.MovOrg.Adapters.Container
{
	public class MovOrgContainerViewModel : OrganizerContainerViewModel
	{
		public MovOrgContainerViewModel(MoviesSectionViewModel moviesSection, ActorsSectionViewModel actorsSection)
		{
			Sections.Add(moviesSection);
			Sections.Add(actorsSection);

			SelectedSection = moviesSection;
		}
	}
}