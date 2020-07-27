using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

using Organizers.Common.Config;

using System;

namespace EntryPoint
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private IConfig config;

		public DbContextFactory(IConfig config)
		{
			this.config = config;
		}

		public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
		{
			if (typeof(TDbContext).Equals(typeof(MoviesContext)))
				return (TDbContext)(IDbContext)CreateDbContext(Array.Empty<string>());

			return Activator.CreateInstance<TDbContext>();
		}

		public MoviesContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<MoviesContext>();
			optionsBuilder.UseSqlServer(config.GetConnectionString())
					.EnableSensitiveDataLogging()
					.UseLoggerFactory(new LoggerFactory(
						new[] { new DebugLoggerProvider() }, new LoggerFilterOptions { MinLevel = LogLevel.Warning }));

			return new MoviesContext(optionsBuilder.Options);
		}
	}
}