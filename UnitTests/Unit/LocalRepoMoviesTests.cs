using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using EntryPoint;

using Infrastructure.MovOrg.EFCore;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Organizers.Common.Config;
using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases.Repositories;

using System;
using System.Linq;
using System.Threading.Tasks;

using Tests.Common;

namespace Tests.Unit
{
	public abstract class LocalRepoMoviesTests : TestData
	{
		protected IDbContextScopeFactory DbContextScopeFactory { get; }
		protected IAmbientDbContextLocator AmbientDbContextLocator { get; }

		protected ILocalMoviesRepository LocalRepo { get; }

		protected IConfig Config { get; set; }

		private MoviesContext MoviesContext
		{
			get
			{
				var dbContext = AmbientDbContextLocator.Get<MoviesContext>();

				if (dbContext == null)
					throw new InvalidOperationException("DbContext has been called outside DbContextScope prior to this");

				return dbContext;
			}
		}

		#region Initialization

		protected LocalRepoMoviesTests(IConfig config)
		{
			Config = config;
			var optionsBuilderMoviesContext = GenerateOptionsBuilderMoviexContext();

			var contextFactory = new DbContextFactory(optionsBuilderMoviesContext);
			DbContextScopeFactory = new DbContextScopeFactory(contextFactory);
			AmbientDbContextLocator = new AmbientDbContextLocator();
			LocalRepo = new EFCoreLocalMoviesRepository(AmbientDbContextLocator, config);
			Seed();
		}

		private DbContextOptionsBuilder<MoviesContext> GenerateOptionsBuilderMoviexContext()
		{
			var options = new DbContextOptionsBuilder<MoviesContext>();
			var connString = Config.GetConnectionString();
			if (Config is UnitTestConfig)
			{
				var connection = new SqliteConnection(connString);
				connection.Open();
				return options.UseSqlite(connection)
					.EnableSensitiveDataLogging();
			}
			else if (Config is IntegrationTestConfig)
				return options.UseSqlServer(connString)
						.EnableSensitiveDataLogging();
			else
				throw new Exception("Unknown config type");
		}

		private void Seed()
		{
			using var context = DbContextScopeFactory.Create();
			MoviesContext.Database.EnsureDeleted();
			MoviesContext.Database.EnsureCreated();

			MoviesContext.Movies.Add(testMovieSeeded);

			MoviesContext.People.Add(testDirectorSeeded);

			MoviesContext.People.Add(testActorSeeded);
			MoviesContext.Movies.Add(testMovieSeededWithDetails);

			context.SaveChanges();
		}

		[TestMethod]
		public void TestDbContextScopeWorks()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var moviesContext = AmbientDbContextLocator.Get<MoviesContext>();
			Assert.IsNotNull(moviesContext.Movies.Find(testMovieSeeded.Id));
		}

		#endregion Initialization

		#region UpdateMovieDetails

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldChangeLastUpdatedDetails_WhenCalled()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = testMovieSeeded;
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(testMovieSeeded.Id);
			Assert.AreEqual(DateTime.Now.ToShortDateString(), existingMovie.LastUpdatedDetails.Value.ToShortDateString());
			Assert.AreEqual(DateTime.Now.ToShortTimeString(), existingMovie.LastUpdatedDetails.Value.ToShortTimeString());
			Assert.IsTrue(existingMovie.AreDetailsAvailable);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieDirector_WhenDirectorExist()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = testMovieSeeded;
			movieUnderTest.DirectorList.Add(
				new MovieDirector
				{
					Movie = movieUnderTest,
					MovieId = movieUnderTest.Id,
					Person = testDirectorSeeded,
					PersonId = testDirectorSeeded.Id
				});

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, testDirectorSeeded.Id);
			Assert.AreEqual(movieUnderTest, existingMovie);
			Assert.IsNotNull(existingRelation);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddDirectorAndMovieDirector_WhenDirectorNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = testMovieSeeded;
			movieUnderTest.DirectorList.Add(
				new MovieDirector
				{
					Movie = movieUnderTest,
					MovieId = movieUnderTest.Id,
					Person = testDirectorNotSeeded,
					PersonId = testDirectorNotSeeded.Id
				});

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, testDirectorNotSeeded.Id);
			var existingDirector = MoviesContext.People.Find(testDirectorNotSeeded.Id);

			Assert.IsNotNull(existingRelation);
			Assert.AreEqual(testDirectorNotSeeded, existingDirector);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddNewMovieDirector_WhenOtherExistsPreviously()
		{
			var movieUnderTest = testMovieSeededWithDetails;

			using var context = DbContextScopeFactory.Create();

			movieUnderTest.DirectorList.Add(
				new MovieDirector
				{
					Movie = movieUnderTest,
					MovieId = movieUnderTest.Id,
					Person = testDirectorNotSeeded,
					PersonId = testDirectorNotSeeded.Id
				});
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, testDirectorSeeded.Id);
			var existingSecondRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, testDirectorNotSeeded.Id);
			var existingSecondDirector = MoviesContext.People.Find(testDirectorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			Assert.IsNotNull(existingFirstRelation);
			Assert.IsNotNull(existingSecondRelation);
			Assert.IsNotNull(existingSecondDirector);
			Assert.AreEqual(movieUnderTest, existingMovie);
		}

		[TestMethod]
		[ExpectedException(typeof(RepositoryException))]
		public async Task UpdateMovieDetails_ShouldThrowException_WhenMovieNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var newMovie = testMovieNotSeeded;

			await LocalRepo.UpdateMovieDetails(newMovie);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddNewMovieActor_WhenOtherExistsPreviously()
		{
			var movieUnderTest = testMovieSeededWithDetails;

			using var context = DbContextScopeFactory.Create();

			movieUnderTest.ActorList.Add(
				new MovieActor
				{
					Movie = movieUnderTest,
					MovieId = movieUnderTest.Id,
					Person = testActorNotSeeded,
					PersonId = testActorNotSeeded.Id
				});
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieActors.Include(x => x.Person).SingleOrDefault(x => x.MovieId == movieUnderTest.Id && x.PersonId == testActorSeeded.Id);
			var existingSecondRelation = MoviesContext.MovieActors.Include(x => x.Person).SingleOrDefault(x => x.MovieId == movieUnderTest.Id && x.PersonId == testActorNotSeeded.Id);
			var existingSecondActor = MoviesContext.People.Find(testActorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			Assert.IsNotNull(existingFirstRelation);
			Assert.IsNotNull(existingSecondRelation);
			Assert.IsNotNull(existingSecondActor);
			Assert.AreEqual(movieUnderTest, existingMovie);
		}

		#endregion UpdateMovieDetails

		[TestMethod]
		public async Task GetMovieById_ShouldGetAllDetails_WhenExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var movieUnderTest = testMovieSeededWithDetails;
			var actualMovie = await LocalRepo.GetMovieDetailsById(movieUnderTest.Id);

			Assert.AreEqual(movieUnderTest, actualMovie);
		}

		[TestMethod]
		public async Task GetAllMovies_ShouldGetAllMovies_WhenExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var movies = await LocalRepo.GetAllMovies();
			Assert.AreEqual(2, movies.Count());
		}

		[TestMethod]
		public async Task AreDetailsAvailable_ShouldCheckIfTheyAre_WhenExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var detailsAvailable = await LocalRepo.AreDetailsAvailableFor(testMovieSeededWithDetails.Id);
			var detailsNotAvailable = await LocalRepo.AreDetailsAvailableFor(testMovieSeeded.Id);

			Assert.IsTrue(detailsAvailable);
			Assert.IsFalse(detailsNotAvailable);
		}
	}
}