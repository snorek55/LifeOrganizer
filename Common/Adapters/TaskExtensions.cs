using System;
using System.Threading.Tasks;

namespace Common.Adapters
{
	public static class TaskExtensions
	{
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void

		public static async void FireAndForgetSafeAsync(this Task task, INotificationHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			try
			{
				await task;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex)
			{
				handler?.NotifyError(ex);
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
}