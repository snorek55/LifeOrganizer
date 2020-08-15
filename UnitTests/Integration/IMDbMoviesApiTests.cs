using Common.Setup;

using EntityFramework.DbContextScope;

using FluentAssertions;

using Main.GUI.Setup;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.MovOrg.APIs;
using MovOrg.Infrastructure.MovOrg.EFCore;
using MovOrg.Organizer.UseCases.Repositories;

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