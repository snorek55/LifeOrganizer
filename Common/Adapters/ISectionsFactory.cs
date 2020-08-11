using System.Collections.Generic;

namespace Common.Adapters
{
	public interface ISectionsFactory
	{
		INotificationHandler ErrorHandler { get; set; }

		IEnumerable<ISectionViewModel> GenerateSections();
	}
}