using System;

namespace Organizers.Common.Adapters
{
	public interface INotificationHandler
	{
		void NotifyError(Exception ex);

		void NotifyError(string error);

		void NotifyStatusMessage(string message);

		void NotifyWaitMessage();
	}
}