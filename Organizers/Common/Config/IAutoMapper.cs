﻿namespace Organizers.Common.Config
{
	public interface IAutoMapper
	{
		TDestination Map<TDestination>(object source);

		TDestination Map<TSource, TDestination>(TSource source);
	}
}