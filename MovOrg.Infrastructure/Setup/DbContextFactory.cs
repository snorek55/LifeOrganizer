using Common.Setup;

using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using MovOrg.Infrastructure.EFCore;

using System;
using System.Diagnostics;

namespace MovOrg.Infrastructure.Setup
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext;
		private IConfig config;

		public DbContextFactory()
		{
			Debug.WriteLine("Default constructor was used on DbContextFactory. This is only allowed for Migrations.");
			config = new Config();
			optionsBuilderMoviesContext = new DbContextOptionsBuilder<MoviesContext>().UseSqlServer(config.GetConnectionString(true))
						.EnableSensitiveDataLogging();
		}

		public DbContextFactory(IConfig config)
		{
			this.config = config;
			optionsBuilderMoviesContext = new DbContextOptionsBuilder<MoviesContext>().UseSqlServer(config.GetConnectionString())
						.EnableSensitiveDataLogging();
		}

		public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
		{
			return (TDbContext)(IDbContext)CreateDbContext(Array.Empty<string>());
		}

		public MoviesContext CreateDbContext(string[] args)
		{
			return new MoviesContext(optionsBuilderMoviesContext.Options);
		}
	}
}