using Organizers.Common.Adapters;

namespace Organizers.MovOrg.Adapters.Items
{
	public class BoxOfficeViewModel : BaseViewModel
	{
		public string Budget { get; set; }

		public string OpeningWeekendUSA { get; set; }

		public string GrossUsa { get; set; }

		public string CumulativeWorldwideGross { get; set; }
	}
}