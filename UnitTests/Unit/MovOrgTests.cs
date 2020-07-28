using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Organizers.Common.Config;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases;
using Organizers.MovOrg.UseCases.Repositories;

using System.Threading.Tasks;

using Tests.Common;

namespace Tests.Unit
{
	[TestClass]
	public class MovOrgTests : TestData
	{
		private IMoviesService moviesService;
		private Mock<IConfig> mockConfig = new Mock<IConfig>();
		private Mock<ILocalMoviesRepository> mockLocalRepo = new Mock<ILocalMoviesRepository>();
		private Mock<IApiMoviesRepository> mockApiRepo = new Mock<IApiMoviesRepository>();
		private IDbContextScopeFactory contextScopeFactory = new DbContextScopeFactory(new TestDbContextFactory(UnitTestConfig));

		public MovOrgTests()
		{
			moviesService = new MoviesService(contextScopeFactory, mockLocalRepo.Object, mockApiRepo.Object, mockConfig.Object);
		}

		[TestMethod]
		public async Task GetMovieWithId_ShouldRetunDetailsFromApi_WhenNotAvailableInLocal()
		{
			var movieTestId = "MovieId";
			var testMovie = new Movie { Id = movieTestId };
			mockLocalRepo.Setup(x => x.AreDetailsAvailableFor(movieTestId)).ReturnsAsync(false);
			mockApiRepo.Setup(x => x.GetMovieDetailsById(movieTestId)).ReturnsAsync(testMovie);

			var response = await moviesService.GetMovieWithId(movieTestId);

			Assert.AreEqual(testMovie.Id, response.Movie.Id);
		}
	}
}