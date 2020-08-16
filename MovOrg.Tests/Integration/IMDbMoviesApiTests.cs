using AutoMapper;

using Common.Setup;

using EntityFramework.DbContextScope;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.APIs;
using MovOrg.Infrastructure.EFCore;
using MovOrg.Infrastructure.Setup;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Tests.Setup;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Tests.Integration
{
	[TestClass]
	public class IMDbMoviesApiTests
	{
		private IMDbMoviesApiRepository imdbApiRepository;
		private ILocalMoviesRepository sqlServerTestRepository;
		private IConfig config;

		public IMDbMoviesApiTests()
		{
			config = new IntegrationTestConfig();
			var AmbientDbContextLocator = new AmbientDbContextLocator();
			sqlServerTestRepository = new EFCoreLocalMoviesRepository(AmbientDbContextLocator, config);
			imdbApiRepository = new IMDbMoviesApiRepository(new MapperImpl(new List<Profile> { new IMDbProfile() }), sqlServerTestRepository, config);
		}

		[TestMethod]
		public async Task GetTopMovies_Returns250Movies()
		{
			var movies = await imdbApiRepository.GetTopMovies();
			movies.Should().HaveCount(250);
		}
	}
}