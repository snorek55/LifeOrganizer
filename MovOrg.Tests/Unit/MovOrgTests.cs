using AutoFixture;

using AutoMapper;

using Common.Setup;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using MovOrg.Infrastructure.Setup;
using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;
using MovOrg.Tests.Setup;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Tests.Unit
{
	[TestClass]
	public class MovOrgTests : TestData
	{
		private IMoviesService moviesService;
		private Mock<IConfig> mockConfig = new Mock<IConfig>();
		private Mock<ILocalMoviesRepository> mockLocalRepo = new Mock<ILocalMoviesRepository>();
		private Mock<IApiMoviesRepository> mockApiRepo = new Mock<IApiMoviesRepository>();
		private IDbContextScopeFactory contextScopeFactory = new DbContextScopeFactory(new TestDbContextFactory(UnitTestConfig));

		private IAutoMapper mapper = new MapperImpl(new List<Profile> { new IMDbProfile() });

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
			var movieDto = mapper.Map<UpdateMovieDetailsDto>(movie);
			mockLocalRepo.Setup(x => x.AreDetailsAvailableFor(movie.Id)).ReturnsAsync(false);
			mockApiRepo.Setup(x => x.GetAllMovieDetailsById(movie.Id)).ReturnsAsync(movieDto);
			var response = await moviesService.GetMovieWithId(movie.Id);
			MovieWithDetailsDto movieWithDetails = movieDto;
			response.Movie.Should().BeEquivalentTo(movieWithDetails);
		}
	}
}