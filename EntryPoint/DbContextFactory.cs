using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System;

namespace EntryPoint
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext;

		public DbContextFactory(DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext)
		{
			this.optionsBuilderMoviesContext = optionsBuilderMoviesContext;
		}

		public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
		{
			if (typeof(TDbContext).Equals(typeof(MoviesContext)))
				return (TDbContext)(IDbContext)CreateDbContext(Array.Empty<string>());

			return Activator.CreateInstance<TDbContext>();
		}

		public MoviesContext CreateDbContext(string[] args)
		{
			return new MoviesContext(optionsBuilderMoviesContext.Options);
		}
	}
}