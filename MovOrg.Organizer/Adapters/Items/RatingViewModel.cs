using Common.Adapters;

namespace MovOrg.Organizer.Adapters.Items
{
	public class RatingViewModel : BaseViewModel
	{
		public string SourceName { get; set; }

		public string SourceLogoUrl { get; set; }

		public string Score { get; set; }
	}
}