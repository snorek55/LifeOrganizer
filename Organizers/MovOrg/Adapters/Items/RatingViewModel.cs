using Common.Adapters;

namespace Organizers.MovOrg.Adapters.Items
{
	public class RatingViewModel : BaseViewModel
	{
		public string SourceName { get; set; }

		public string SourceLogoUrl { get; set; }

		public string Score { get; set; }
	}
}