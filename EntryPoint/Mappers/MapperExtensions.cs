using AutoMapper;

using System;
using System.Linq.Expressions;

namespace EntryPoint.Mapper
{
	public static class MapperExtensions
	{
		public static IMappingExpression<TSource, TDestination> IgnoreDestinationMember<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
		{
			map.ForMember(selector, config => config.Ignore());
			return map;
		}

		public static IMappingExpression<TSource, TDestination> IgnoreSourceMember<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TSource, object>> selector)
		{
			map.ForSourceMember(selector, config => config.DoNotValidate());
			return map;
		}
	}
}