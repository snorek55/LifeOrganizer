using AutoMapper;
using AutoMapper.Configuration;

using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Setup
{
	public class MapperImpl : IAutoMapper
	{
		private IMapper mapper;

		public MapperImpl(MapperConfigurationExpression configExpression)
		{
			CreateMapper(configExpression);
		}

		public MapperImpl(List<Profile> profiles)
		{
			var configExpression = new MapperConfigurationExpression();
			configExpression.AddProfiles(profiles);
			CreateMapper(configExpression);
		}

		private void CreateMapper(MapperConfigurationExpression configExpression)
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
	}
}