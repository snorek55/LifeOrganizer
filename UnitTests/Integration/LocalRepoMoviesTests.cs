using Domain;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using EntryPoint;

using Infrastructure.EFCore;
using Infrastructure.MovOrg.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Organizers.Common.UseCases;
using Organizers.MovOrg.UseCases.Repositories;

using System;
using System.Linq;
using System.Threading.Tasks;

using Tests;

using UnitTests;

namespace Integration
{
	public abstract class LocalRepoMoviesTests : TestData
	{
		protected DbContextOptions<MoviesContext> ContextOptions { get; }
		protected IDbContextScopeFactory DbContextScopeFactory { get; }
		protected IAmbientDbContextLocator AmbientDbContextLocator { get; }

		protected ILocalMoviesRepository LocalRepo { get; }

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

		protected LocalRepoMoviesTests(DbContextOptions<MoviesContext> contextOptions)
		{
			ContextOptions = contextOptions;
			var contextFactory = new TestDbContextFactory(new TestConfig());
			DbContextScopeFactory = new DbContextScopeFactory(contextFactory);
			AmbientDbContextLocator = new AmbientDbContextLocator();
			LocalRepo = new EFCoreLocalMoviesRepository(AmbientDbContextLocator);
			Seed();
		}

		private void Seed()
		{
			using var context = DbContextScopeFactory.Create();

			MoviesContext.Database.EnsureDeleted();
			MoviesContext.Database.EnsureCreated();

			MoviesContext.Movies.Add(testMovieSeeded);

			MoviesContext.Directors.Add(testDirectorSeeded);

			MoviesContext.Actors.Add(testActorSeeded);
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
					Director = testDirectorSeeded,
					DirectorId = testDirectorSeeded.Id
				});

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MoviesDirectors.Find(movieUnderTest.Id, testDirectorSeeded.Id);
			Assert.IsNotNull(existingMovie);
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
					Director = testDirectorNotSeeded,
					DirectorId = testDirectorNotSeeded.Id
				});

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingRelation = MoviesContext.MoviesDirectors.Find(movieUnderTest.Id, testDirectorNotSeeded.Id);
			var existingDirector = MoviesContext.Directors.Find(testDirectorNotSeeded.Id);

			Assert.IsNotNull(existingRelation);
			Assert.IsNotNull(existingDirector);
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
					Director = testDirectorNotSeeded,
					DirectorId = testDirectorNotSeeded.Id
				});
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MoviesDirectors.Find(movieUnderTest.Id, testDirectorSeeded.Id);
			var existingSecondRelation = MoviesContext.MoviesDirectors.Find(movieUnderTest.Id, testDirectorNotSeeded.Id);
			var existingSecondDirector = MoviesContext.Directors.Find(testDirectorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			Assert.IsNotNull(existingFirstRelation);
			Assert.IsNotNull(existingSecondRelation);
			Assert.IsNotNull(existingSecondDirector);
			Assert.AreEqual(2, existingMovie.DirectorList.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(RepositoryException))]
		public async Task UpdateMovieDetails_ShouldThrowException_WhenMovieNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var newMovie = testMovieNotSeeded;

			await LocalRepo.UpdateMovieDetails(newMovie);
		}

		//TODO: make sure all data is persisted
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
					Actor = testActorNotSeeded,
					ActorId = testActorNotSeeded.Id
				});
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MoviesActors.Include(x => x.Actor).SingleOrDefault(x => x.MovieId == movieUnderTest.Id && x.ActorId == testActorSeeded.Id);
			var existingSecondRelation = MoviesContext.MoviesActors.Include(x => x.Actor).SingleOrDefault(x => x.MovieId == movieUnderTest.Id && x.ActorId == testActorNotSeeded.Id);
			var existingSecondActor = MoviesContext.Actors.Find(testActorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			Assert.IsNotNull(existingFirstRelation);
			Assert.IsNotNull(existingSecondRelation);
			Assert.IsNotNull(existingSecondActor);
			Assert.AreEqual(2, existingMovie.ActorList.Count);
			Assert.IsNotNull(existingFirstRelation.Actor);
			Assert.IsNotNull(existingSecondRelation.Actor);
		}

		#endregion UpdateMovieDetails

		[TestMethod]
		public async Task GetMovieById_ShouldGetAllDetails_WhenExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var movieUnderTest = testMovieSeededWithDetails;
			var actualMovie = await LocalRepo.GetMovieDetailsById(movieUnderTest.Id);

			//TODO: should make sure all details are in there, I should make a comparer or something...
			Assert.AreEqual(movieUnderTest.Id, actualMovie.Id);
			Assert.AreEqual(movieUnderTest.DirectorList.Count, actualMovie.DirectorList.Count);
			Assert.AreEqual(movieUnderTest.ActorList.Count, actualMovie.ActorList.Count);
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