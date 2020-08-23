using AutoMapper;
using AutoMapper.Configuration;

using System.Diagnostics;
using System.Linq;

namespace Common.Setup
{
	public class MapperImpl : IAutoMapper
	{
		private IMapper mapper;

		public MapperImpl(params Profile[] profiles) : base()
		{
			var configExpression = new MapperConfigurationExpression();
			configExpression.AddProfiles(profiles);
			InitializeMapper(configExpression);
		}

		private void InitializeMapper(MapperConfigurationExpression configExpression)
		{
			var config = new MapperConfiguration(configExpression);
			try
			{
				config.AssertConfigurationIsValid();
			}
			catch (AutoMapperConfigurationException ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
			mapper = config.CreateMapper();
		}

		public TDestination Map<TSource, TDestination>(TSource source)
		{
			return mapper.Map<TSource, TDestination>(source);
		}

		public TDestination Map<TDestination>(object source)
		{
			try
			{
				return mapper.Map<TDestination>(source);
			}
			catch (AutoMapperMappingException ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
		{
			return mapper.ProjectTo<TDestination>(source);
		}
	}
}