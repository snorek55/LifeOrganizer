using System;

namespace Organizers.Common.Adapters
{
	public interface INotificationHandler
	{
		void NotifyError(Exception ex);

		void NotifyError(string error);

		void NotifyStatus(string message);

		void NotifyWait();

		void NotifyItemCount(int count);

		void ClearAllInfo();
	}
}