using Common.Setup;

using EntityFramework.DbContextScope;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.APIs;
using MovOrg.Infrastructure.EFCore;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Tests.Setup;

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
			sqlServerTestRepository = new EFCoreLocalMoviesRepository(AmbientDbContextLocator, config, TestData.Mapper);
			imdbApiRepository = new IMDbMoviesApiRepository(TestData.Mapper, sqlServerTestRepository, config);
		}

		[TestMethod]
		public async Task GetTopMovies_Returns250Movies()
		{
			var movies = await imdbApiRepository.GetTopMovies();
			movies.Should().HaveCount(250);
		}
	}
}