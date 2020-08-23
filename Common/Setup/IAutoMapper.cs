using System.Linq;

namespace Common.Setup
{
	public interface IAutoMapper
	{
		TDestination Map<TDestination>(object source);

		TDestination Map<TSource, TDestination>(TSource source);

		IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source);
	}
}