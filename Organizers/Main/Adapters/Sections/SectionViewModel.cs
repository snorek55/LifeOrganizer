using Organizers.Common.Adapters;

namespace Organizers.Main.Adapters.Sections
{
	public class SectionViewModel : BaseViewModel
	{
		public string SectionName { get; set; }

		private INotificationHandler notificationHandler;

		public INotificationHandler NotificationsHandler
		{
			get
			{
				if (notificationHandler == null)
					notificationHandler = new BaseErrorHandler();
				return notificationHandler;
			}
			set
			{
				notificationHandler = value;
			}
		}
	}
}