using Common.Adapters;
using Common.Extensions;

using System.Linq;

namespace MovOrg.Organizers.Adapters.Container
{
	public class MovOrgContainerViewModel : BaseOrganizerContainerViewModel
	{
		public MovOrgContainerViewModel(ISectionsFactory sectionsFactory)
		{
			sectionsFactory.ErrorHandler = this;

			var sections = sectionsFactory.GenerateSections();

			Sections.AddRange(sections);

			SelectedSection = Sections.FirstOrDefault();
		}
	}
}