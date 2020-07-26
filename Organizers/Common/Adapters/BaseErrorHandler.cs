﻿using System;
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

		public void NotifyItemCount(int count)
		{
			Debug.WriteLine("updated count" + count);
		}

		public void NotifyStatus(string message)
		{
			Debug.WriteLine(message);
		}

		public void NotifyWait()
		{
			Debug.WriteLine("Processing, please wait...");
		}
	}
}