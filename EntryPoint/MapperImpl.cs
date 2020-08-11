using AutoMapper;

using Common.Setup;

using Infrastructure.Setup;

using MovOrg.Organizers.Setup;

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
				cfg.AddProfile<MovOrgIMDbProfile>();
				cfg.AddProfile<MovOrgViewModelsProfile>();
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