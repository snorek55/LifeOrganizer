using Common.Adapters;

using MovOrg.Organizer.Adapters.Sections;

using System.Collections.Generic;

namespace MovOrg.Organizer.Adapters.Container
{
	public class MovOrgSectionsFactory : ISectionsFactory
	{
		public INotificationHandler ErrorHandler { get; set; }

		private MoviesSectionViewModel moviesSection;
		private ActorsSectionViewModel actorsSection;

		public MovOrgSectionsFactory(MoviesSectionViewModel moviesSectionViewModel, ActorsSectionViewModel actorsSectionViewModel)
		{
			moviesSection = moviesSectionViewModel;
			actorsSection = actorsSectionViewModel;
		}

		public IEnumerable<ISectionViewModel> GenerateSections()
		{
			moviesSection.NotificationsHandler = ErrorHandler;
			actorsSection.NotificationsHandler = ErrorHandler;

			return new List<ISectionViewModel>
			{
				moviesSection,
				actorsSection
			};
		}
	}
}