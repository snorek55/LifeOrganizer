using Common.Setup;

using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.EFCore.DbAccess
{
	public class MoviesListsDbAccess : IMoviesListsDbAccess
	{
		//TODO: create base class
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

		private readonly IAutoMapper mapper;

		public MoviesListsDbAccess(IAmbientDbContextLocator ambientDbContextLocator, IAutoMapper mapper)
		{
			this.ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException(nameof(ambientDbContextLocator));
			this.mapper = mapper;
		}

		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromLocal()
		{
			return await mapper.ProjectTo<MovieListItemDto>(DbContext.Movies).AsNoTracking().ToListAsync();
		}

		public async Task UpdateSuggestedTitleMovies(IEnumerable<MovieListItemDto> movies)
		{
			foreach (var movie in movies)
			{
				var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
				if (persistentMovie == null)
					DbContext.Movies.Add(mapper.Map<Movie>(movie));
			}
		}

		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle)
		{
			return await mapper.ProjectTo<MovieListItemDto>(DbContext.Movies).AsNoTracking().Where(x => EF.Functions.Like(x.Title, $"%{suggestedTitle}%")).ToListAsync();
		}

		public async Task UpdateTopMovies(IEnumerable<MovieListItemDto> topApiMovies)
		{
			await DbContext.Movies.Where(x => x.LastUpdatedTop250 != null).ForEachAsync(x => { x.LastUpdatedTop250 = null; x.Rank = null; });

			foreach (var movie in topApiMovies)
			{
				var persistentMovie = await DbContext.Movies.FindAsync(movie.Id);
				if (persistentMovie == null)
				{
					var entryMovie = DbContext.Movies.Add(mapper.Map<Movie>(movie));
					entryMovie.Entity.LastUpdatedTop250 = DateTime.Now;
				}
				else
				{
					persistentMovie.LastUpdatedTop250 = DateTime.Now;
					persistentMovie.Rank = movie.Rank;
				}
			}
		}
	}
}