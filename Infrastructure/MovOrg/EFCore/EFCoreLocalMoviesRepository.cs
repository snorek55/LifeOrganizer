using Domain;

using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;

using Organizers.Common.Domain;
using Organizers.Common.UseCases;
using Organizers.MovOrg.UseCases.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.MovOrg.EFCore
{
	public class EFCoreLocalMoviesRepository : ILocalMoviesRepository
	{
		private readonly IAmbientDbContextLocator ambientDbContextLocator;

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

		public EFCoreLocalMoviesRepository(IAmbientDbContextLocator ambientDbContextLocator)
		{
			this.ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException(nameof(ambientDbContextLocator));
		}

		public async Task<bool> AreDetailsAvailableFor(string id)
		{
			return (await DbContext.Movies.FindAsync(id)).AreDetailsAvailable;
		}

		public async Task<IEnumerable<Movie>> GetAllMovies()
		{
			return await DbContext.Movies.ToListAsync();
		}

		public async Task<Movie> GetMovieDetailsById(string id)
		{
			var movie = await DbContext.Movies
			.Include(x => x.BoxOffice)
			.Include(x => x.Trailer)
			.Include(x => x.Ratings)
				.ThenInclude(x => x.RatingSource)
			.Include(x => x.ActorList)
				.ThenInclude(x => x.Actor)
			.Include(x => x.CompanyList)
				.ThenInclude(x => x.Company)
			.Include(x => x.DirectorList)
				.ThenInclude(x => x.Director)
			.Include(x => x.WriterList)
				.ThenInclude(x => x.Writer)
			.SingleOrDefaultAsync(x => x.Id == id);

			return movie;
		}

		public async Task UpdateMovieDetails(Movie movie)
		{
			await UpdateMovie(movie);
			var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
			persistentMovie.LastUpdatedDetails = DateTime.Now;
			movie.LastUpdatedDetails = DateTime.Now;
		}

		public async Task UpdateSuggestedTitleMovies(IEnumerable<Movie> movies)
		{
			foreach (var movie in movies)
			{
				var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
				if (persistentMovie == null)
					DbContext.Movies.Add(movie);
			}
		}

		public async Task UpdateTopMovies(IEnumerable<Movie> topApiMovies)
		{
			await DbContext.Movies.Where(x => x.LastUpdatedTop250 != null).ForEachAsync(x => { x.LastUpdatedTop250 = null; x.Rank = null; });

			foreach (var movie in topApiMovies)
			{
				var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
				if (persistentMovie == null)
				{
					var newMovie = DbContext.Movies.Add(movie);
					newMovie.Entity.LastUpdatedTop250 = DateTime.Now;
				}
				else
				{
					persistentMovie.LastUpdatedTop250 = DateTime.Now;
					persistentMovie.Rank = movie.Rank;
				}
			}
		}

		public async Task<IEnumerable<RatingSource>> GetRatingSources()
		{
			return await DbContext.RatingSources.ToListAsync();
		}

		private async Task UpdateMovie(Movie movie)
		{
			var existingMovie = await DbContext.Movies
				.Include(x => x.BoxOffice)
				.Include(x => x.Trailer)
				.Include(x => x.Ratings)
				.Include(x => x.ActorList)
				.Include(x => x.CompanyList)
				.Include(x => x.DirectorList)
				.Include(x => x.WriterList)
				.SingleOrDefaultAsync(x => x.Id == movie.Id);
			//TODO: better way?
			if (existingMovie == null) throw new RepositoryException("Movie does not exists in local. Cannot persist details.");

			var lastUpdatedTop = existingMovie.LastUpdatedTop250;
			var rank = existingMovie.Rank;
			var lastUpdatedDetails = existingMovie.LastUpdatedDetails;
			DbContext.Entry(existingMovie).CurrentValues.SetValues(movie);
			existingMovie.LastUpdatedTop250 = lastUpdatedTop;
			existingMovie.Rank = rank;
			existingMovie.LastUpdatedDetails = lastUpdatedDetails;

			UpdateManyToManyLinkAndEntities(movie.DirectorList);
			UpdateManyToManyLinkAndEntities(movie.ActorList);
			UpdateManyToManyLinkAndEntities(movie.WriterList);
			UpdateManyToManyLinkAndEntities(movie.CompanyList);

			existingMovie.BoxOffice = movie.BoxOffice;
			existingMovie.Trailer = movie.Trailer;
			//existingMovie.Ratings = movie.Ratings;

			foreach (var rating in movie.Ratings)
			{
				Rating existingRating = DbContext.Ratings.Find(rating.Id, rating.RatingSourceId);
				if (existingRating == null)
				{
					rating.Movie = existingMovie;
					DbContext.Ratings.Add(rating);
					existingRating = DbContext.Ratings.Find(rating.Id, rating.RatingSourceId);
				}

				existingMovie.Ratings.Add(existingRating);
			}
		}

		//TODO: extract to efcoreutils
		private void UpdateManyToManyLinkAndEntities<T>(IEnumerable<T> disconnectedLinksEnumerable) where T : class
		{
			var linkType = typeof(T);
			Type movieType = typeof(Movie);
			Type entityType = null;
			PropertyInfo movieIdPropertyInfo = linkType.GetProperty("MovieId");
			PropertyInfo entityIdPropertyInfo = null;
			PropertyInfo entityPropertyInfo = null;
			PropertyInfo linkListMoviePropertyInfo = null;

			foreach (var prop in linkType.GetProperties())
			{
				if (prop.Name.Equals("MovieId") || prop.Name.Equals("Movie"))
					continue;

				if (prop.Name.EndsWith("Id"))
					entityIdPropertyInfo = linkType.GetProperty(prop.Name);
				else
				{
					entityType = prop.PropertyType;
					entityPropertyInfo = linkType.GetProperty(prop.Name);
				}
			}
			var entityListType = typeof(List<>).MakeGenericType(linkType);
			foreach (var prop in movieType.GetProperties())
			{
				if (prop.PropertyType.Equals(entityListType))
					linkListMoviePropertyInfo = prop;
			}

			//TODO: improve guards
			if (entityType == null || linkType.GetProperties().Length != 4) throw new ArgumentException("Error while trying to get types of many-to-many link entities");

			//TODO: extension method for dbcontext
			var linkDbSet = DbContext.GetType().GetMethod("Set").MakeGenericMethod(linkType).Invoke(DbContext, null);
			var movieDbSet = DbContext.GetType().GetMethod("Set").MakeGenericMethod(movieType).Invoke(DbContext, null);
			var entityDbSet = DbContext.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(DbContext, null);

			var linkDbSetFindMethod = linkDbSet.GetType().GetMethod("Find");
			var movieDbSetFindMethod = movieDbSet.GetType().GetMethod("Find");
			var entityDbSetFindMethod = entityDbSet.GetType().GetMethod("Find");

			foreach (var link in disconnectedLinksEnumerable)
			{
				var movieId = (string)movieIdPropertyInfo.GetValue(link);
				var entityId = (string)entityIdPropertyInfo.GetValue(link);
				var existingLink = linkDbSetFindMethod.Invoke(linkDbSet, new object[] { new object[] { movieId, entityId } });
				if (existingLink != null)
					continue;
				UpdateEntity((Entity)entityPropertyInfo.GetValue(link));
				var existingEntity = entityDbSetFindMethod.Invoke(entityDbSet, new object[] { new object[] { entityId } });
				if (existingEntity == null) throw new RepositoryException("Entity not added correctly when updating many to many link");
				var existingMovie = movieDbSetFindMethod.Invoke(movieDbSet, new object[] { new object[] { movieId } });
				if (existingMovie == null) throw new RepositoryException("Movie not found when updating many to many link");

				var newLink = Activator.CreateInstance(linkType);
				linkType.GetProperty("MovieId").SetValue(newLink, movieId);
				linkType.GetProperty("Movie").SetValue(newLink, existingMovie);
				entityIdPropertyInfo.SetValue(newLink, entityId);
				entityPropertyInfo.SetValue(newLink, existingEntity);
				var existingMovieLinkList = linkListMoviePropertyInfo.GetValue(existingMovie);
				typeof(List<>).MakeGenericType(linkType).GetMethod("Add").Invoke(existingMovieLinkList, new object[] { newLink });
			}
		}

		//TODO: https://stackoverflow.com/questions/16153047/net-invoke-async-method-and-await
		private void UpdateEntity(Entity entity)
		{
			var superEntityType = entity.GetType();
			var dbSet = DbContext.GetType().GetMethod("Set").MakeGenericMethod(superEntityType).Invoke(DbContext, null);
			var parameterArray = new object[] { new object[] { entity.Id } };
			var dbsetType = dbSet.GetType();
			var findMethod = dbsetType.GetMethod("Find");

			var existingEntity = findMethod.Invoke(dbSet, parameterArray);
			if (existingEntity == null)
			{
				var addMethod = dbsetType.GetMethod("Add");
				addMethod.Invoke(dbSet, new object[] { entity });
			}
		}

		public void MarkMovieAsFavorite(string id, bool isFavorite)
		{
			var existingMovie = DbContext.Movies.Find(id);
			existingMovie.IsFavorite = isFavorite;
		}

		public void MarkMovieAsMustWatch(string id, bool isMustWatch)
		{
			var existingMovie = DbContext.Movies.Find(id);
			existingMovie.IsMustWatch = isMustWatch;
		}

		public void MarkMovieAsWatched(string id, bool isWatched)
		{
			var existingMovie = DbContext.Movies.Find(id);
			existingMovie.IsWatched = isWatched;
		}

		public Task<Director> GetDirectorDetails(string id)
		{
			throw new NotImplementedException();
		}
	}
}