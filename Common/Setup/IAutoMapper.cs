using AutoMapper;

namespace Common.Setup
{
	public interface IAutoMapper
	{
		IMapper Mapper { get; }

		TDestination Map<TDestination>(object source);

		TDestination Map<TSource, TDestination>(TSource source);
	}
}