namespace Common.Adapters
{
	public interface ISectionViewModel
	{
		string SectionName { get; }
		INotificationHandler NotificationsHandler { get; set; }
	}
}