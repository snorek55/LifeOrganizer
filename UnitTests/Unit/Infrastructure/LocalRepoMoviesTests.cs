﻿using AutoFixture;

using Common.Setup;
using Common.UseCases;

using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

using FluentAssertions;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.MovOrg.EFCore;
using MovOrg.Infrastructure.Setup;
using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tests.Common;

namespace Tests.Unit.Infrastructure
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

		protected LocalRepoMoviesTests(IConfig config) : base()
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

			MoviesContext.Movies.Add(TestMovieSeededWithoutRelatedInfo);

			MoviesContext.People.Add(TestDirectorSeeded);
			MoviesContext.People.Add(TestActorSeeded);
			MoviesContext.People.Add(TestWriterSeeded);
			MoviesContext.Companies.Add(TestCompanySeeded);
			MoviesContext.MovieImageDatas.Add(TestMovieImageSeeded);
			MoviesContext.Ratings.Add(TestRatingSeeded);

			MoviesContext.Movies.Add(TestMovieSeededWithRelatedInfo);

			MoviesContext.RatingSources.AddRange(RatingSources);
			context.SaveChanges();
		}

		[TestMethod]
		public void TestDbContextScopeWorks()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var moviesContext = AmbientDbContextLocator.Get<MoviesContext>();
			Assert.IsNotNull(moviesContext.Movies.Find(TestMovieSeededWithoutRelatedInfo.Id));
		}

		#endregion Initialization

		#region UpdateMovieDetails

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldChangeLastUpdatedDetails_WhenCalled()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(TestMovieSeededWithoutRelatedInfo.Id);
			existingMovie.LastUpdatedDetails.Value.ToString("dd/MM/yy HH:mm:ss").Should().Be(DateTime.Now.ToString("dd/MM/yy HH:mm:ss"));
			existingMovie.AreDetailsAvailable.Should().BeTrue();
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieDirector_WhenDirectorExist()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieDirector
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestDirectorSeeded,
				PersonId = TestDirectorSeeded.Id
			};
			movieUnderTest.DirectorList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, TestDirectorSeeded.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddDirectorAndMovieDirector_WhenDirectorNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieDirector
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestDirectorNotSeeded,
				PersonId = TestDirectorNotSeeded.Id
			};
			movieUnderTest.DirectorList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, TestDirectorNotSeeded.Id);
			var existingDirector = MoviesContext.People.Find(TestDirectorNotSeeded.Id);

			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
			existingDirector.Should().Be(TestDirectorNotSeeded);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddSecondMovieDirector_WhenOtherExistsPreviously()
		{
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var secondRelationUnderTest = new MovieDirector
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestDirectorNotSeeded,
				PersonId = TestDirectorNotSeeded.Id
			};

			using var context = DbContextScopeFactory.Create();
			movieUnderTest.DirectorList.Add(secondRelationUnderTest);
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, TestDirectorSeeded.Id);
			var existingSecondRelation = MoviesContext.MovieDirectors.Find(movieUnderTest.Id, TestDirectorNotSeeded.Id);
			var existingSecondPerson = MoviesContext.People.Find(TestDirectorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);

			existingFirstRelation.Should().BeEquivalentTo(TestMovieDirectorSeeded);
			existingSecondRelation.Should().BeEquivalentTo(secondRelationUnderTest);
			existingSecondPerson.Should().Be(TestDirectorNotSeeded);
			existingMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieWriter_WhenWriterExist()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieWriter
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestWriterSeeded,
				PersonId = TestWriterSeeded.Id
			};
			movieUnderTest.WriterList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieWriters.Find(movieUnderTest.Id, TestWriterSeeded.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddWriterAndMovieWriter_WhenWriterNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieWriter
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestWriterNotSeeded,
				PersonId = TestWriterNotSeeded.Id
			};
			movieUnderTest.WriterList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieWriters.Find(movieUnderTest.Id, TestWriterNotSeeded.Id);
			var existingWriter = MoviesContext.People.Find(TestWriterNotSeeded.Id);

			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
			existingWriter.Should().Be(TestWriterNotSeeded);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddSecondMovieWriter_WhenOtherExistsPreviously()
		{
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var secondRelationUnderTest = new MovieWriter
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestWriterNotSeeded,
				PersonId = TestWriterNotSeeded.Id
			};

			using var context = DbContextScopeFactory.Create();
			movieUnderTest.WriterList.Add(secondRelationUnderTest);
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieWriters.Find(movieUnderTest.Id, TestWriterSeeded.Id);
			var existingSecondRelation = MoviesContext.MovieWriters.Find(movieUnderTest.Id, TestWriterNotSeeded.Id);
			var existingSecondPerson = MoviesContext.People.Find(TestWriterNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);

			existingFirstRelation.Should().BeEquivalentTo(TestMovieWriterSeeded);
			existingSecondRelation.Should().BeEquivalentTo(secondRelationUnderTest);
			existingSecondPerson.Should().Be(TestWriterNotSeeded);
			existingMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieCompany_WhenCompanyExist()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieCompany
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Company = TestCompanySeeded,
				CompanyId = TestCompanySeeded.Id
			};
			movieUnderTest.CompanyList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieCompanies.Find(movieUnderTest.Id, TestCompanySeeded.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddCompanyAndMovieCompany_WhenCompanyNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieCompany
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Company = TestCompanyNotSeeded,
				CompanyId = TestCompanyNotSeeded.Id
			};
			movieUnderTest.CompanyList.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieCompanies.Find(movieUnderTest.Id, TestCompanyNotSeeded.Id);
			var existingCompany = MoviesContext.Companies.Find(TestCompanyNotSeeded.Id);

			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
			existingCompany.Should().Be(TestCompanyNotSeeded);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddSecondMovieCompany_WhenOtherExistsPreviously()
		{
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var secondRelationUnderTest = new MovieCompany
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Company = TestCompanyNotSeeded,
				CompanyId = TestCompanyNotSeeded.Id
			};

			using var context = DbContextScopeFactory.Create();
			movieUnderTest.CompanyList.Add(secondRelationUnderTest);
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieCompanies.Find(movieUnderTest.Id, TestCompanySeeded.Id);
			var existingSecondRelation = MoviesContext.MovieCompanies.Find(movieUnderTest.Id, TestCompanyNotSeeded.Id);
			var existingSecondCompany = MoviesContext.Companies.Find(TestCompanyNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);

			existingFirstRelation.Should().BeEquivalentTo(TestMovieCompanySeeded);
			existingSecondRelation.Should().BeEquivalentTo(secondRelationUnderTest);
			existingSecondCompany.Should().Be(TestCompanyNotSeeded);
			existingMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieSimilar_WhenMovieExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieSimilar
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Similar = TestMovieSeededWithRelatedInfo,
				SimilarId = TestMovieSeededWithRelatedInfo.Id
			};
			movieUnderTest.Similars.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieSimilars.Find(movieUnderTest.Id, TestMovieSeededWithRelatedInfo.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddMovieAndMovieSimilar_WhenMovieNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new MovieSimilar
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Similar = TestMovieNotSeeded,
				SimilarId = TestMovieNotSeeded.Id
			};
			movieUnderTest.Similars.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieSimilars.Find(movieUnderTest.Id, TestMovieNotSeeded.Id);
			var existingSimilar = MoviesContext.Movies.Find(TestMovieNotSeeded.Id);

			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
			existingSimilar.Should().Be(TestMovieNotSeeded);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddSecondMovieSimilar_WhenOtherExistsPreviously()
		{
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var secondRelationUnderTest = new MovieSimilar
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Similar = TestMovieNotSeeded,
				SimilarId = TestMovieNotSeeded.Id
			};

			using var context = DbContextScopeFactory.Create();
			movieUnderTest.Similars.Add(secondRelationUnderTest);
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieSimilars.Find(movieUnderTest.Id, TestMovieSeededWithoutRelatedInfo.Id);
			var existingSecondRelation = MoviesContext.MovieSimilars.Find(movieUnderTest.Id, TestMovieNotSeeded.Id);
			var existingSecondSimilar = MoviesContext.Movies.Find(TestMovieNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);

			existingFirstRelation.Should().BeEquivalentTo(TestMovieSimilarSeeded);
			existingSecondRelation.Should().BeEquivalentTo(secondRelationUnderTest);
			existingSecondSimilar.Should().Be(TestMovieNotSeeded);
			existingMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddImages()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = TestMovieImageNotSeeded;
			relationUnderTest.Movie = movieUnderTest;
			relationUnderTest.MovieId = movieUnderTest.Id;

			movieUnderTest.Images.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.MovieImageDatas.Find(movieUnderTest.Id, TestMovieImageNotSeeded.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddRating_WhenMovieExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var relationUnderTest = new Rating
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Source = RatingSources[0],
				SourceId = RatingSources[0].Id
			};
			movieUnderTest.Ratings.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.Ratings.Find(movieUnderTest.Id, RatingSources[0].Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldAddRatingSourceAndRating_WhenRatingSourceNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var movieUnderTest = TestMovieSeededWithoutRelatedInfo;
			var newRatingSource = fixture.Build<RatingSource>()
				.With(x => x.Ratings, new List<Rating>())
				.Create();
			var relationUnderTest = new Rating
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Source = newRatingSource,
				SourceId = newRatingSource.Id
			};
			movieUnderTest.Ratings.Add(relationUnderTest);

			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);
			var existingRelation = MoviesContext.Ratings.Find(movieUnderTest.Id, newRatingSource.Id);
			var existingRatingSource = MoviesContext.RatingSources.Find(newRatingSource.Id);
			existingMovie.Should().Be(movieUnderTest);
			existingRelation.Should().BeEquivalentTo(relationUnderTest);
			existingRatingSource.Should().Be(newRatingSource);
		}

		[TestMethod]
		[ExpectedException(typeof(RepositoryException))]
		public async Task UpdateMovieDetails_ShouldThrowException_WhenMovieNotExists()
		{
			using var context = DbContextScopeFactory.Create();
			var newMovie = TestMovieNotSeeded;

			await LocalRepo.UpdateMovieDetails(newMovie);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShoulAddNewMovieActor_WhenOtherExistsPreviously()
		{
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var secondRelationUnderTest = new MovieActor
			{
				Movie = movieUnderTest,
				MovieId = movieUnderTest.Id,
				Person = TestActorNotSeeded,
				PersonId = TestActorNotSeeded.Id,
				AsCharacter = "Paco"
			};

			using var context = DbContextScopeFactory.Create();

			movieUnderTest.ActorList.Add(secondRelationUnderTest);
			await LocalRepo.UpdateMovieDetails(movieUnderTest);
			context.SaveChanges();

			var existingFirstRelation = MoviesContext.MovieActors.Find(movieUnderTest.Id, TestActorSeeded.Id);
			var existingSecondRelation = MoviesContext.MovieActors.Find(movieUnderTest.Id, TestActorNotSeeded.Id);
			var existingSecondPerson = MoviesContext.People.Find(TestActorNotSeeded.Id);
			var existingMovie = MoviesContext.Movies.Find(movieUnderTest.Id);

			existingFirstRelation.Should().BeEquivalentTo(TestMovieActorSeeded);
			existingSecondRelation.Should().BeEquivalentTo(secondRelationUnderTest);
			existingSecondPerson.Should().Be(TestActorNotSeeded);
			existingMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldUpdateBoxOffice()
		{
			TestBoxOfficeSeeded.GrossUSA = "11111";

			using var context = DbContextScopeFactory.Create();

			TestMovieSeededWithoutRelatedInfo.BoxOffice = TestBoxOfficeSeeded;

			await LocalRepo.UpdateMovieDetails(TestMovieSeededWithoutRelatedInfo);
			context.SaveChanges();

			var existingMovie = MoviesContext.Movies.Find(TestMovieSeededWithoutRelatedInfo.Id);

			existingMovie.BoxOffice.GrossUSA.Should().Be("11111");
		}

		[TestMethod]
		public async Task UpdateMovieDetails_ShouldUpdateTrailer()
		{
			TestTrailerSeeded.ThumbnailUrl = "11111";

			using var context = DbContextScopeFactory.Create();

			TestMovieSeededWithoutRelatedInfo.Trailer = TestTrailerSeeded;

			await LocalRepo.UpdateMovieDetails(TestMovieSeededWithoutRelatedInfo);
			context.SaveChanges();

			var existingMovie = MoviesContext.Movies.Find(TestMovieSeededWithoutRelatedInfo.Id);

			existingMovie.Trailer.ThumbnailUrl.Should().Be("11111");
		}

		#endregion UpdateMovieDetails

		[TestMethod]
		public async Task GetMovieById_ShouldGetAllDetails_WhenMovieExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var movieUnderTest = TestMovieSeededWithRelatedInfo;
			var actualMovie = await LocalRepo.GetMovieDetailsById(movieUnderTest.Id);

			actualMovie.Should().Be(movieUnderTest);
		}

		[TestMethod]
		public async Task GetAllMovies_ShouldGetAllMovies()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var movies = await LocalRepo.GetAllMovies();
			movies.Should().HaveCount(2);
			movies.Should().BeEquivalentTo(InitialMoviesInDb);
		}

		[TestMethod]
		public async Task AreDetailsAvailable_ShouldCheckIfTheyAre_WhenMovieExistsInDb()
		{
			using var context = DbContextScopeFactory.CreateReadOnly();
			var detailsAvailable = await LocalRepo.AreDetailsAvailableFor(TestMovieSeededWithRelatedInfo.Id);
			var detailsNotAvailable = await LocalRepo.AreDetailsAvailableFor(TestMovieSeededWithoutRelatedInfo.Id);

			Assert.IsTrue(detailsAvailable);
			Assert.IsFalse(detailsNotAvailable);
		}
	}
}