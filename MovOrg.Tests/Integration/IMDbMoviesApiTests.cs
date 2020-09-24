using Common.Setup;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.APIs;
using MovOrg.Tests.Setup;

using System.Threading.Tasks;

namespace MovOrg.Tests.Integration
{
	[TestClass]
	public class IMDbMoviesApiTests
	{
		private ImdbMoviesListsApiAccess imdbApiRepository;
		private IConfig config;

		public IMDbMoviesApiTests()
		{
			config = new IntegrationTestConfig();
			imdbApiRepository = new ImdbMoviesListsApiAccess(TestData.Mapper, config);
		}

		[TestMethod]
		public async Task GetTopMovies_Returns250Movies()
		{
			var movies = await imdbApiRepository.GetTopMovies();
			movies.Should().HaveCount(250);
		}
	}
}