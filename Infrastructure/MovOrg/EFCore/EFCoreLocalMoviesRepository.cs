﻿using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.Common;
using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;

using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
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
			return await DbContext.Movies
							.Include(x => x.BoxOffice)
							.Include(x => x.Trailer)
							.ToListAsync();
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

			if (existingMovie == null) throw new RepositoryException("Movie does not exists in local. Cannot update details.");

			var lastUpdatedTop = existingMovie.LastUpdatedTop250;
			var rank = existingMovie.Rank;
			var lastUpdatedDetails = existingMovie.LastUpdatedDetails;
			DbContext.Entry(existingMovie).CurrentValues.SetValues(movie);
			existingMovie.LastUpdatedTop250 = lastUpdatedTop;
			existingMovie.Rank = rank;
			existingMovie.LastUpdatedDetails = lastUpdatedDetails;
			UpdateRelatedInfo(movie);

			UpdateRatings(movie, existingMovie);
		}

		private void UpdateRelatedInfo(Movie movie)
		{
			movie.BoxOffice = movie.BoxOffice;
			movie.Trailer = movie.Trailer;
			EfCoreUtils.UpdateManyToManyLinkAndEntities(movie.DirectorList, DbContext, typeof(Movie), typeof(Director));
			EfCoreUtils.UpdateManyToManyLinkAndEntities(movie.ActorList, DbContext, typeof(Movie), typeof(Actor));
			EfCoreUtils.UpdateManyToManyLinkAndEntities(movie.WriterList, DbContext, typeof(Movie), typeof(Writer));
			EfCoreUtils.UpdateManyToManyLinkAndEntities(movie.CompanyList, DbContext, typeof(Movie), typeof(Company));
		}

		private void UpdateRatings(Movie movie, Movie existingMovie)
		{
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