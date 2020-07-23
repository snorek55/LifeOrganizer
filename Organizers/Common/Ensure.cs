using System;
using System.Runtime.CompilerServices;

namespace Organizers.Common
{
	public static class Ensure
	{
		public static void IsNotNull(object argument, [CallerArgumentExpression("argument")] string propertyName = default)
		{
			_ = argument ?? throw new ArgumentNullException(propertyName);
		}
	}
}