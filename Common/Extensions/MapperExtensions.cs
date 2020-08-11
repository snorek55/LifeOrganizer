using AutoMapper;

using System;
using System.Linq.Expressions;

namespace Common.Extensions
{
	public static class MapperExtensions
	{
		public static IMappingExpression<TSource, TDestination> IgnoreDestinationMember<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
		{
			return map.ForMember(selector, config => config.Ignore()); ;
		}

		public static IMappingExpression<TSource, TDestination> IgnoreSourceMember<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TSource, object>> selector)
		{
			return map.ForSourceMember(selector, config => config.DoNotValidate()); ;
		}

		public static IMappingExpression<TSource, TDestination> MapFrom<TSource, TDestination, TMember, TSourceMember>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, TMember>> destinationMember, Expression<Func<TSource, TSourceMember>> mapExpression)
		{
			return map.ForMember(destinationMember, opt => opt.MapFrom(mapExpression));
		}
	}
}