using Organizers.Main.Adapters;
using Organizers.Main.Adapters.Sections;

namespace Organizers.GamesOrg
{
	public class GamesOrgContainerViewModel : OrganizerContainerViewModel
	{
		public GamesOrgContainerViewModel(ISectionsFactory sectionsFactory) : base(sectionsFactory)
		{
			Sections.Add(new GamesSectionViewModel());
			Sections.Add(new CompaniesSectionViewModel());
		}
	}
}