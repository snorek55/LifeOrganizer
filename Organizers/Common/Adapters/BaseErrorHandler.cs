using System;
using System.Diagnostics;

namespace Organizers.Common.Adapters
{
	internal class BaseErrorHandler : INotificationHandler
	{
		public void NotifyError(Exception ex)
		{
			Debug.WriteLine(ex);
		}

		public void NotifyError(string error)
		{
			Debug.WriteLine(error);
		}

		public void NotifyStatusMessage(string message)
		{
			Debug.WriteLine(message);
		}

		public void NotifyWaitMessage()
		{
			Debug.WriteLine("Processing, please wait...");
		}
	}
}