using System;
using System.Diagnostics;

namespace Organizers.Common.Adapters
{
	internal class BaseErrorHandler : IErrorHandler
	{
		public void HandleError(Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}
}