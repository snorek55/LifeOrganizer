using Common.Adapters;
using Common.UseCases;

using System;
using System.Threading.Tasks;

namespace Common.Adapters
{
	public class SectionViewModel : BaseViewModel, ISectionViewModel
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
				NotifyStatus(string.Empty);
			}
			else
			{
				NotifyStatus(okMessage);
			}

			return result;
		}

		protected void NotifyStatus(string message)
		{
			NotificationsHandler.NotifyStatus(message);
		}

		protected void NotifyError(string error)
		{
			NotificationsHandler.NotifyError(error);
		}

		protected void NotifyException(Exception ex)
		{
			NotifyError(ex.ToString());
		}

		protected void NotifyWait()
		{
			NotificationsHandler.NotifyWait();
		}

		protected void NotifyItemTotal(int count)
		{
			NotificationsHandler.NotifyItemCount(count);
		}
	}
}