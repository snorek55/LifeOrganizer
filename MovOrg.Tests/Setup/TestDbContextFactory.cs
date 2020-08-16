using Common.Setup;

using EntityFramework.DbContextScope.Interfaces;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using MovOrg.Infrastructure.EFCore;

using System;

namespace MovOrg.Tests.Setup
{
	public class TestDbContextFactory : IDbContextFactory
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilder;

		public TestDbContextFactory(IConfig config)
		{
			optionsBuilder = new DbContextOptionsBuilder<MoviesContext>();
			var connString = config.GetConnectionString();
			if (config is UnitTestConfig)
			{
				var connection = new SqliteConnection(connString);
				connection.Open();
				optionsBuilder.UseSqlite(connection)
					.EnableSensitiveDataLogging();
			}
			else if (config is IntegrationTestConfig)
				optionsBuilder.UseSqlServer(connString)
					   .EnableSensitiveDataLogging();
			else
				throw new Exception("Unknown config type");
		}

		public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
		{
			if (typeof(TDbContext).Equals(typeof(MoviesContext)))
				return (TDbContext)Activator.CreateInstance(typeof(MoviesContext), optionsBuilder.Options);

			return Activator.CreateInstance<TDbContext>();
		}
	}
}