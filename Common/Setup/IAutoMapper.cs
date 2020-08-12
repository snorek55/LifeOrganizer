using AutoMapper.Configuration;

namespace Common.Setup
{
	public interface IAutoMapper
	{
		//TODO: mapperconfigurationexpression must be interfaced or smt
		void CreateMapper(MapperConfigurationExpression configExpression);

		TDestination Map<TDestination>(object source);

		TDestination Map<TSource, TDestination>(TSource source);
	}
}