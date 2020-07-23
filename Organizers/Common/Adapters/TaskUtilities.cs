using System;
using System.Threading.Tasks;

namespace Organizers.Common.Adapters
{
	public static class TaskUtilities
	{
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void

		internal static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			try
			{
				await task;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex)
			{
				handler?.HandleError(ex);
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
}