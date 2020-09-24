using Common.Setup;

using EntityFramework.DbContextScope.Interfaces;

using System;

namespace MovOrg.Infrastructure.EFCore
{
	public abstract class BaseMovieDbAccess
	{
		protected readonly IAmbientDbContextLocator ambientDbContextLocator;

		protected readonly IAutoMapper mapper;

		protected MoviesContext DbContext
		{
			get
			{
				var dbContext = ambientDbContextLocator.Get<MoviesContext>();

				if (dbContext == null)
					throw new InvalidOperationException("DbContext has been called outside DbContextScope prior to this");

				return dbContext;
			}
		}

		protected BaseMovieDbAccess(IAmbientDbContextLocator ambientDbContextLocator, IAutoMapper mapper)
		{
			this.ambientDbContextLocator = ambientDbContextLocator;
			this.mapper = mapper;
		}
	}
}