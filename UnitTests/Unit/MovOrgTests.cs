using AutoFixture;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using FluentAssertions;

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

		[TestInitialize]
		public void Initialize()
		{
			fixture = new Fixture();
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}

		[TestMethod]
		public async Task GetMovieWithId_ShouldRetunDetailsFromApi_WhenNotAvailableInLocal()
		{
			var movie = fixture.Create<Movie>();
			mockLocalRepo.Setup(x => x.AreDetailsAvailableFor(movie.Id)).ReturnsAsync(false);
			mockApiRepo.Setup(x => x.GetMovieDetailsById(movie.Id)).ReturnsAsync(movie);

			var response = await moviesService.GetMovieWithId(movie.Id);

			response.Movie.Should().Be(movie);
		}
	}
}