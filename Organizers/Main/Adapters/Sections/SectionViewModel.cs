using Organizers.Common.Adapters;
using Organizers.Common.UseCases;

using System;
using System.Threading.Tasks;

namespace Organizers.Main.Adapters.Sections
{
	public class SectionViewModel : BaseViewModel
	{
		public string SectionName { get; set; }

		private INotificationHandler notificationsHandler;

		public INotificationHandler NotificationsHandler
		{
			get
			{
				if (notificationsHandler == null)
					notificationsHandler = new BaseErrorHandler();
				return notificationsHandler;
			}
			set
			{
				notificationsHandler = value;
			}
		}

		protected async Task<T> ExecuteCommandTaskAsync<T>(Func<Task<T>> action, string okMessage) where T : ResponseBase, new()
		{
			NotifyWait();
			var result = await action();

			if (result.HasError)
			{
				NotifyError(result.Error);
			}
			else
			{
				NotifyStatus(okMessage);
			}

			return result;
		}

		protected void NotifyStatus(string message)
		{
			notificationsHandler.NotifyStatus(message);
		}

		protected void NotifyError(string error)
		{
			notificationsHandler.NotifyError(error);
		}

		protected void NotifyException(Exception ex)
		{
			notificationsHandler.NotifyError(ex);
		}

		protected void NotifyWait()
		{
			notificationsHandler.NotifyWait();
		}

		protected void NotifyItemTotal(int count)
		{
			NotificationsHandler.NotifyItemCount(count);
		}
	}
}