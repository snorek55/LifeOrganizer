using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Diagnostics;

namespace Infrastructure.MovOrg.EFCore
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext;

		public DbContextFactory()
		{
			Debug.WriteLine("Default constructor was used on DbContextFactory. This is only allowed for Migrations.");
			optionsBuilderMoviesContext = new DbContextOptionsBuilder<MoviesContext>().UseSqlServer(GetConnectionString())
						.EnableSensitiveDataLogging();
		}

		public DbContextFactory(DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext)
		{
			this.optionsBuilderMoviesContext = optionsBuilderMoviesContext;
		}

		private static string GetConnectionString()
		{
			//Workaround for Migrations because ConfigurationManager returns null
			var configuration = new ConfigurationBuilder()
			 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			 .AddXmlFile(@"Properties/App.config")
			 .Build();
			var connString = configuration.GetValue<string>("connectionStrings:add:SqlServerConnectionString:connectionString");
			return connString;
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