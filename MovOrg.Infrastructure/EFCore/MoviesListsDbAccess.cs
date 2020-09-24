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
	public class MoviesListsDbAccess : BaseMovieDbAccess, IMoviesListsDbAccess
	{
		public MoviesListsDbAccess(IAmbientDbContextLocator ambientDbContextLocator, IAutoMapper mapper) : base(ambientDbContextLocator, mapper)
		{
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

		public async Task<IEnumerable<RatingSource>> GetRatingSources()
		{
			return await mapper.ProjectTo<RatingSource>(DbContext.RatingSources).AsNoTracking().ToListAsync();
		}
	}
}