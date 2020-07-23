using Organizers.Main.Adapters;

namespace Organizers.GamesOrg
{
	public class GamesOrgContainerViewModel : OrganizerContainerViewModel
	{
		public GamesOrgContainerViewModel()
		{
			Sections.Add(new GamesSectionViewModel());
			Sections.Add(new CompaniesSectionViewModel());
		}
	}
}