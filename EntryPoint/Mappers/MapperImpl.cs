﻿using AutoMapper;

using EntryPoint.Mapper.Profiles;

using Organizers.Common.Config;

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
			return mapper.Map<TDestination>(source);
		}
	}
}