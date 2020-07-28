using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Organizers.Common.Config;

using System;
using System.Diagnostics;

namespace EntryPoint
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext;
		private IConfig migrationsConfig = new Config();

		public DbContextFactory()
		{
			Debug.WriteLine("Default constructor was used on DbContextFactory. This is only allowed for Migrations.");
			optionsBuilderMoviesContext = new DbContextOptionsBuilder<MoviesContext>().UseSqlServer(migrationsConfig.GetConnectionString())
						.EnableSensitiveDataLogging();
		}

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