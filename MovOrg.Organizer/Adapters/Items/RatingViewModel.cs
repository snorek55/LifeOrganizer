using Common.Adapters;

namespace MovOrg.Organizers.Adapters.Items
{
	public class RatingViewModel : BaseViewModel
	{
		public string SourceName { get; set; }

		public string SourceLogoUrl { get; set; }

		public string Score { get; set; }
	}
}