using AutoFixture;

using Common.Config;

using Organizers.MovOrg.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Common
{
	public class TestData
	{
		public static IConfig UnitTestConfig { get; } = new UnitTestConfig();
		public static IConfig IntegrationConfig { get; } = new IntegrationTestConfig();

		protected Fixture fixture = new Fixture();

		protected Movie TestMovieSeededWithoutRelatedInfo { get; private set; }
		protected Movie TestMovieSeededWithRelatedInfo { get; private set; }
		protected Movie TestMovieNotSeeded { get; private set; }

		protected Person TestDirectorSeeded { get; private set; }
		protected Person TestDirectorNotSeeded { get; private set; }

		protected Person TestActorSeeded { get; private set; }
		protected Person TestActorNotSeeded { get; private set; }

		protected Person TestWriterSeeded { get; private set; }
		protected Person TestWriterNotSeeded { get; private set; }

		protected Company TestCompanySeeded { get; private set; }
		protected Company TestCompanyNotSeeded { get; private set; }

		protected BoxOffice TestBoxOfficeNotSeeded { get; private set; }
		protected BoxOffice TestBoxOfficeSeeded { get; private set; }

		protected Trailer TestTrailerSeeded { get; private set; }
		protected Trailer TestTrailerNotSeeded { get; private set; }

		protected MovieDirector TestMovieDirectorSeeded { get; private set; }
		protected MovieActor TestMovieActorSeeded { get; private set; }
		protected MovieWriter TestMovieWriterSeeded { get; private set; }
		protected MovieCompany TestMovieCompanySeeded { get; private set; }
		protected MovieSimilar TestMovieSimilarSeeded { get; private set; }

		protected MovieImageData TestMovieImageSeeded { get; private set; }
		protected MovieImageData TestMovieImageNotSeeded { get; private set; }

		protected List<Movie> InitialMoviesInDb { get; } = new List<Movie>();

		protected List<RatingSource> RatingSources { get; private set; }

		protected Rating TestRatingSeeded { get; private set; }
		protected Rating TestRatingNotSeeded { get; private set; }

		public TestData()
		{
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());

			CreateTestMovies();

			CreateTestRelatedData();

			CreateTestOneToOneData();

			CreateTestManyToManyData();

			TestMovieSeededWithRelatedInfo.LastUpdatedDetails = DateTime.Now;

			InitialMoviesInDb.Add(TestMovieSeededWithoutRelatedInfo);
			InitialMoviesInDb.Add(TestMovieSeededWithRelatedInfo);
		}

		private void CreateTestOneToOneData()
		{
			TestBoxOfficeNotSeeded = fixture.Build<BoxOffice>()
				.With(x => x.Movie, TestMovieNotSeeded)
				.With(x => x.Id, TestMovieNotSeeded.Id)
				.Create();
			TestBoxOfficeSeeded = fixture.Build<BoxOffice>()
				.With(x => x.Id, TestMovieSeededWithoutRelatedInfo.Id)
				.With(x => x.Movie, TestMovieSeededWithoutRelatedInfo)
				.Create();

			TestTrailerNotSeeded = fixture.Build<Trailer>()
				.With(x => x.Movie, TestMovieNotSeeded)
				.With(x => x.Id, TestMovieNotSeeded.Id)
				.Create();
			TestTrailerSeeded = fixture.Build<Trailer>()
				.With(x => x.Id, TestMovieSeededWithoutRelatedInfo.Id)
				.With(x => x.Movie, TestMovieSeededWithoutRelatedInfo)
				.Create();

			TestRatingSeeded = fixture.Build<Rating>()
				.With(x => x.Source, RatingSources[0])
				.With(x => x.SourceId, RatingSources[0].Id)
				.With(x => x.Movie, TestMovieSeededWithRelatedInfo)
				.With(x => x.MovieId, TestMovieSeededWithRelatedInfo.Id)
				.Create();

			TestRatingNotSeeded = fixture.Build<Rating>()
			.With(x => x.Source, RatingSources[1])
			.With(x => x.SourceId, RatingSources[1].Id)
			.With(x => x.Movie, TestMovieNotSeeded)
			.With(x => x.MovieId, TestMovieNotSeeded.Id)
			.Create();

			TestMovieSeededWithRelatedInfo.BoxOffice = TestBoxOfficeSeeded;
			TestMovieSeededWithRelatedInfo.Ratings.Add(TestRatingSeeded);
		}

		private void CreateTestManyToManyData()
		{
			TestMovieDirectorSeeded = new MovieDirector
			{
				Person = TestDirectorSeeded,
				PersonId = TestDirectorSeeded.Id,
				Movie = TestMovieSeededWithRelatedInfo,
				MovieId = TestMovieSeededWithRelatedInfo.Id
			};
			TestMovieActorSeeded = new MovieActor
			{
				Person = TestActorSeeded,
				PersonId = TestActorSeeded.Id,
				Movie = TestMovieSeededWithRelatedInfo,
				MovieId = TestMovieSeededWithRelatedInfo.Id,
				AsCharacter = "Test"
			};
			TestMovieWriterSeeded = new MovieWriter
			{
				Person = TestWriterSeeded,
				PersonId = TestWriterSeeded.Id,
				Movie = TestMovieSeededWithRelatedInfo,
				MovieId = TestMovieSeededWithRelatedInfo.Id,
			};
			TestMovieCompanySeeded = new MovieCompany
			{
				Company = TestCompanySeeded,
				CompanyId = TestCompanySeeded.Id,
				Movie = TestMovieSeededWithRelatedInfo,
				MovieId = TestMovieSeededWithRelatedInfo.Id,
			};
			TestMovieImageSeeded = fixture.Build<MovieImageData>()
				.With(x => x.Movie, TestMovieSeededWithRelatedInfo)
				.With(x => x.MovieId, TestMovieSeededWithRelatedInfo.Id)
				.Create();
			TestMovieImageNotSeeded = fixture.Build<MovieImageData>()
				.With(x => x.Movie, TestMovieNotSeeded)
				.With(x => x.MovieId, TestMovieNotSeeded.Id)
				.Create();

			TestMovieSimilarSeeded = new MovieSimilar
			{
				Similar = TestMovieSeededWithoutRelatedInfo,
				SimilarId = TestMovieSeededWithoutRelatedInfo.Id,
				Movie = TestMovieSeededWithRelatedInfo,
				MovieId = TestMovieSeededWithRelatedInfo.Id,
			};

			TestMovieSeededWithRelatedInfo.DirectorList.Add(TestMovieDirectorSeeded);
			TestMovieSeededWithRelatedInfo.ActorList.Add(TestMovieActorSeeded);
			TestMovieSeededWithRelatedInfo.WriterList.Add(TestMovieWriterSeeded);
			TestMovieSeededWithRelatedInfo.CompanyList.Add(TestMovieCompanySeeded);
			TestMovieSeededWithRelatedInfo.Images.Add(TestMovieImageSeeded);
			TestMovieSeededWithRelatedInfo.Similars.Add(TestMovieSimilarSeeded);
		}

		private void CreateTestRelatedData()
		{
			TestDirectorSeeded = CreatePersonWithoutRelatedInfo();
			TestDirectorNotSeeded = CreatePersonWithoutRelatedInfo();

			TestActorNotSeeded = CreatePersonWithoutRelatedInfo();
			TestActorSeeded = CreatePersonWithoutRelatedInfo();

			TestWriterNotSeeded = CreatePersonWithoutRelatedInfo();
			TestWriterSeeded = CreatePersonWithoutRelatedInfo();

			TestCompanyNotSeeded = CreateCompanyWithoutRelatedInfo();
			TestCompanySeeded = CreateCompanyWithoutRelatedInfo();

			RatingSources = fixture.CreateMany<RatingSource>().ToList();
			RatingSources.ForEach(x => x.Ratings = new List<Rating>());
		}

		private void CreateTestMovies()
		{
			TestMovieSeededWithoutRelatedInfo = CreateMovieWithoutRelatedInfo();
			TestMovieSeededWithoutRelatedInfo.LastUpdatedDetails = null;

			TestMovieSeededWithRelatedInfo = CreateMovieWithoutRelatedInfo();
			TestMovieSeededWithRelatedInfo.LastUpdatedDetails = DateTime.Now;

			TestMovieNotSeeded = CreateMovieWithoutRelatedInfo();
		}

		private Movie CreateMovieWithoutRelatedInfo()
		{
			return fixture.Build<Movie>()
						.Without(x => x.ActorList)
						.Without(x => x.DirectorList)
						.Without(x => x.WriterList)
						.Without(x => x.BoxOffice)
						.Without(x => x.Trailer)
						.Without(x => x.CompanyList)
						.Without(x => x.Images)
						.Without(x => x.Similars)
						.Without(x => x.Ratings)
						.Create();
		}

		private Person CreatePersonWithoutRelatedInfo()
		{
			return fixture.Build<Person>()
						.Create();
		}

		private Company CreateCompanyWithoutRelatedInfo()
		{
			return fixture.Build<Company>()
						.Create();
		}
	}
}