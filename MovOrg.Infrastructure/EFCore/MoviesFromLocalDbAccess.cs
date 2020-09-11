using Common.Setup;

using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.EFCore.DbAccess
{
	public class MoviesFromLocalDbAccess : IMoviesFromLocalDbAccess
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

		public MoviesFromLocalDbAccess(IAmbientDbContextLocator ambientDbContextLocator, IAutoMapper mapper)
		{
			this.ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException(nameof(ambientDbContextLocator));
			this.mapper = mapper;
		}

		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromLocal()
		{
			return await mapper.ProjectTo<MovieListItemDto>(DbContext.Movies).AsNoTracking().ToListAsync();
		}
	}
}