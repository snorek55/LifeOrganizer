using Organizers.Common.Adapters;

using System.Collections.Generic;

namespace Organizers.Main.Adapters.Sections
{
	public interface ISectionsFactory
	{
		INotificationHandler ErrorHandler { get; set; }

		List<SectionViewModel> GenerateMoviesSections();

		List<SectionViewModel> GenerateGamesSections();
	}
}