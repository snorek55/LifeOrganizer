using Microsoft.EntityFrameworkCore;

using System;

namespace Infrastructure.Common
{
	internal static class EFCoreExtensions
	{
		public static object GetDbSetWithReflection(this DbContext dbContext, Type linkType)
		{
			return dbContext.GetType().GetMethod("Set").MakeGenericMethod(linkType).Invoke(dbContext, null);
		}
	}
}