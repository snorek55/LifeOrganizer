using EntityFramework.DbContextScope;

using EntryPoint;
using EntryPoint.Mapper;

using FluentAssertions;

using Infrastructure.MovOrg.APIs;
using Infrastructure.MovOrg.EFCore;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Organizers.UseCases.Repositories;

using System.Threading.Tasks;

using Tests.Common;

namespace Tests.Integration
{
	[TestClass]
	public class IMDbMoviesApiTests
	{
		private IMDbMoviesApiRepository imdbApiRepository;
		private ILocalMoviesRepository sqlServerTestRepository;
		private Config config;

		public IMDbMoviesApiTests()
		{
			config = new IntegrationTestConfig();
			var AmbientDbContextLocator = new AmbientDbContextLocator();
			sqlServerTestRepository = new EFCoreLocalMoviesRepository(AmbientDbContextLocator, config);
			imdbApiRepository = new IMDbMoviesApiRepository(new MapperImpl(), sqlServerTestRepository, config);
		}

		[TestMethod]
		public async Task GetTopMovies_Returns250Movies()
		{
			var movies = await imdbApiRepository.GetTopMovies();
			movies.Should().HaveCount(250);
		}
	}
}