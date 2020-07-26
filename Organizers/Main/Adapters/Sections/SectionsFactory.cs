using Organizers.Common.Adapters;
using Organizers.MovOrg.Adapters.Sections;

using System.Collections.Generic;

namespace Organizers.Main.Adapters.Sections
{
	public class SectionsFactory : ISectionsFactory
	{
		private MoviesSectionViewModel moviesSection;
		private ActorsSectionViewModel actorsSection;
		public INotificationHandler ErrorHandler { get; set; }

		public SectionsFactory(MoviesSectionViewModel moviesSection, ActorsSectionViewModel actorsSection)
		{
			this.moviesSection = moviesSection;
			this.actorsSection = actorsSection;
		}

		public List<SectionViewModel> GenerateGamesSections()
		{
			throw new System.NotImplementedException();
		}

		public List<SectionViewModel> GenerateMoviesSections()
		{
			var sections = new List<SectionViewModel>();
			sections.Add(moviesSection);
			sections.Add(actorsSection);

			foreach (var s in sections)
			{
				s.NotificationsHandler = ErrorHandler;
			}

			return sections;
		}
	}
}