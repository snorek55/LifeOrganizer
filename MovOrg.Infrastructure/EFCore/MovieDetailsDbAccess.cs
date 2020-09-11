using Common.Setup;
using Common.UseCases;

using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.EFCore.DbAccess
{
	public class MovieDetailsDbAccess : IMovieDetailsDbAccess
	{
		//TODO: create base class
		private readonly IAmbientDbContextLocator ambientDbContextLocator;

		private readonly IAutoMapper mapper;

		private MoviesContext DbContext
		{
			get
			{
				var dbContext = ambientDbContextLocator.Get<MoviesContext>();

				if (dbContext == null)
					throw new InvalidOperationException("DbContext has been called outside DbContextScope prior to this");

				return dbContext;
			}
		}

		private readonly IConfig config;

		public MovieDetailsDbAccess(IAmbientDbContextLocator ambientDbContextLocator, IConfig config, IAutoMapper mapper)
		{
			this.ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException(nameof(ambientDbContextLocator));
			this.mapper = mapper;
			this.config = config;
		}

		public async Task<MovieWithDetailsDto> GetMovieDetails(string id)
		{
			var movieWithDetails = await mapper.ProjectTo<MovieWithDetailsDto>(DbContext.Movies.Where(x => x.Id == id)).AsNoTracking().FirstOrDefaultAsync();
			return movieWithDetails;
		}

		public async Task<bool> AreDetailsAvailableFor(string movieId)
		{
			return (await DbContext.Movies.FindAsync(movieId)).AreDetailsAvailable;
		}

		public async Task AddMovieDetails(UpdateMovieDetailsDto movie)
		{
			await UpdateMovie(movie);
			var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
			persistentMovie.LastUpdatedDetails = RoundToSecond(DateTime.Now);
		}

		private async Task AddAndSaveDefaultRatingSources()
		{
			var list = await DbContext.RatingSources.ToListAsync();
			var count = 1;
			foreach (var source in Enum.GetNames(typeof(DefaultRatingSources)))
			{
				if (list.Find(x => x.Name == source) == null)
				{
					DbContext.Add(
								new RatingSource
								{
									Id = count.ToString(),
									Name = source,
									LogoUrl = config.GetRatingSourceLogoUrl(source)
								}); ;
				}
				count++;
			}

			DbContext.SaveChanges();
		}

		private async Task UpdateMovie(UpdateMovieDetailsDto movie)
		{
			//TODO: this makes update very slow. must improve
			var existingMovie = await DbContext.Movies
				.Include(x => x.BoxOffice)
				.Include(x => x.Trailer)
				.Include(x => x.Ratings)
				.Include(x => x.ActorList)
					.ThenInclude(x => x.Person)
				.Include(x => x.DirectorList)
					.ThenInclude(x => x.Person)
				.Include(x => x.WriterList)
					.ThenInclude(x => x.Person)
				.Include(x => x.CompanyList)
				.Include(x => x.Images)
				.Include(x => x.Similars)
					.ThenInclude(x => x.Similar)
				.SingleOrDefaultAsync(x => x.Id == movie.Id);

			if (existingMovie == null) throw new RepositoryException("Movie does not exists in local. Cannot update details.");

			var lastUpdatedTop = existingMovie.LastUpdatedTop250;
			var rank = existingMovie.Rank;
			var lastUpdatedDetails = existingMovie.LastUpdatedDetails;
			var mustWatch = existingMovie.IsMustWatch;
			var watched = existingMovie.IsWatched;
			var fav = existingMovie.IsFavorite;
			DbContext.Entry(existingMovie).CurrentValues.SetValues(movie);
			existingMovie.LastUpdatedTop250 = lastUpdatedTop;
			existingMovie.Rank = rank;
			existingMovie.LastUpdatedDetails = lastUpdatedDetails;
			existingMovie.IsMustWatch = mustWatch;
			existingMovie.IsWatched = watched;
			existingMovie.IsFavorite = fav;
			existingMovie.BoxOffice = movie.BoxOffice;
			existingMovie.Trailer = movie.Trailer;

			UpdateRelatedInfo(movie);

			UpdateRatings(movie, existingMovie);
		}

		private void UpdateRelatedInfo(UpdateMovieDetailsDto movie)
		{
			foreach (var image in movie.Images)
			{
				var existingLink = DbContext.MovieImageDatas.Find(new object[] { image.MovieId, image.Id });
				if (existingLink != null)
					continue;

				var existingMovie = DbContext.Movies.Find(new object[] { image.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				existingMovie.Images.Add(new MovieImageData
				{
					Id = image.Id,
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Image = image.Image,
					Title = image.Title
				});
			}

			//TODO: refactor this
			foreach (var moviePerson in movie.ActorList)
			{
				var existingLink = DbContext.MovieActors.Find(new object[] { moviePerson.MovieId, moviePerson.PersonId });
				if (existingLink != null)
					continue;

				var existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				if (existingPerson == null)
				{
					DbContext.People.Add(moviePerson.Person);
					existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				}

				var existingMovie = DbContext.Movies.Find(new object[] { moviePerson.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				var newLink = new MovieActor
				{
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Person = existingPerson,
					PersonId = existingPerson.Id,
					AsCharacter = moviePerson.AsCharacter,
					IsStar = moviePerson.IsStar
				};

				DbContext.MovieActors.Add(newLink);
			}

			foreach (var moviePerson in movie.WriterList)
			{
				var existingLink = DbContext.MovieWriters.Find(new object[] { moviePerson.MovieId, moviePerson.PersonId });
				if (existingLink != null)
					continue;

				var existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				if (existingPerson == null)
				{
					DbContext.People.Add(moviePerson.Person);
					existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				}

				var existingMovie = DbContext.Movies.Find(new object[] { moviePerson.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				var newLink = new MovieWriter
				{
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Person = existingPerson,
					PersonId = existingPerson.Id,
				};

				DbContext.MovieWriters.Add(newLink);
			}

			foreach (var moviePerson in movie.DirectorList)
			{
				var existingLink = DbContext.MovieDirectors.Find(new object[] { moviePerson.MovieId, moviePerson.PersonId });
				if (existingLink != null)
					continue;

				var existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				if (existingPerson == null)
				{
					DbContext.People.Add(moviePerson.Person);
					existingPerson = DbContext.People.Find(new object[] { moviePerson.PersonId });
				}

				var existingMovie = DbContext.Movies.Find(new object[] { moviePerson.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				var newLink = new MovieDirector
				{
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Person = existingPerson,
					PersonId = existingPerson.Id,
				};

				DbContext.MovieDirectors.Add(newLink);
			}

			foreach (var movieSimilar in movie.Similars)
			{
				var existingLink = DbContext.MovieSimilars.Find(new object[] { movieSimilar.MovieId, movieSimilar.SimilarId });
				if (existingLink != null)
					continue;

				var existingSimilar = DbContext.Movies.Find(new object[] { movieSimilar.SimilarId });
				if (existingSimilar == null)
				{
					DbContext.Movies.Add(movieSimilar.Similar);
					existingSimilar = DbContext.Movies.Find(new object[] { movieSimilar.SimilarId });
				}

				var existingMovie = DbContext.Movies.Find(new object[] { movieSimilar.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				var newLink = new MovieSimilar
				{
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Similar = existingSimilar,
					SimilarId = existingSimilar.Id,
				};

				DbContext.MovieSimilars.Add(newLink);
			}

			foreach (var movieCompany in movie.CompanyList)
			{
				var existingLink = DbContext.MovieCompanies.Find(new object[] { movieCompany.MovieId, movieCompany.CompanyId });
				if (existingLink != null)
					continue;

				var existingCompany = DbContext.Companies.Find(new object[] { movieCompany.CompanyId });
				if (existingCompany == null)
				{
					DbContext.Companies.Add(movieCompany.Company);
					existingCompany = DbContext.Companies.Find(new object[] { movieCompany.CompanyId });
				}

				var existingMovie = DbContext.Movies.Find(new object[] { movieCompany.MovieId });
				if (existingMovie == null) throw new InvalidOperationException("Se esperaba encontrar pelicula para agregar personas");

				var newLink = new MovieCompany
				{
					Movie = existingMovie,
					MovieId = existingMovie.Id,
					Company = existingCompany,
					CompanyId = existingCompany.Id,
				};

				DbContext.MovieCompanies.Add(newLink);
			}
		}

		private void UpdateRatings(UpdateMovieDetailsDto movie, Movie existingMovie)
		{
			foreach (var ratingDto in movie.Ratings)
			{
				var existingRating = DbContext.Ratings.Find(ratingDto.MovieId, ratingDto.SourceId);
				var existingSource = DbContext.RatingSources.Find(ratingDto.SourceId);

				if (existingSource == null)
				{
					DbContext.RatingSources.Add(ratingDto.Source);
					existingSource = DbContext.RatingSources.Find(ratingDto.SourceId);
				}

				if (existingRating == null)
				{
					var rating = new Rating
					{
						Movie = existingMovie,
						MovieId = existingMovie.Id,
						Source = existingSource,
						SourceId = existingSource.Id,
					};

					DbContext.Ratings.Add(rating);
					existingRating = DbContext.Ratings.Find(ratingDto.MovieId, ratingDto.SourceId);
				}

				var isOk = float.TryParse(ratingDto.Score, out float f);
				if (isOk)
					existingRating.Score = f;
				else
					existingRating.Score = null;

				var ratingAlreadyAdded = existingMovie.Ratings.Find(x => x.SourceId == existingRating.SourceId);
				if (ratingAlreadyAdded == null)
					existingMovie.Ratings.Add(existingRating);
				else
					ratingAlreadyAdded.Score = existingRating.Score;
			}
		}

		private DateTime RoundToSecond(DateTime dateTime)
		{
			var stringDateTime = dateTime.ToString("dd/MM/yy HH:mm:ss");
			return DateTime.Parse(stringDateTime, new CultureInfo("es-es"));
		}

		public async Task<IEnumerable<MovieImageDto>> GetMovieImages(string id)
		{
			var images = await DbContext.MovieImageDatas.Where(x => x.MovieId == id).ToListAsync();
			var imagesDto = mapper.Map<List<MovieImageDto>>(images);
			return imagesDto;
		}
	}
}