using System;

namespace Organizers.Common.Adapters
{
	internal interface IErrorHandler
	{
		void HandleError(Exception ex);
	}
}