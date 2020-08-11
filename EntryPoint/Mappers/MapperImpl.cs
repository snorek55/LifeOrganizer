using AutoMapper;

using Common.Config;

using EntryPoint.Mapper.Profiles;

using System.Diagnostics;

namespace EntryPoint.Mapper
{
	public class MapperImpl : IAutoMapper
	{
		private IMapper mapper;

		public MapperImpl()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<IMDbProfile>();
				cfg.AddProfile<ViewModelsProfile>();
			});

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