using EntityFramework.DbContextScope.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using MovOrg.Infrastructure.EFCore;

using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace MovOrg.Infrastructure.Setup
{
	public class DbContextFactory : IDbContextFactory, IDesignTimeDbContextFactory<MoviesContext>
	{
		private DbContextOptionsBuilder<MoviesContext> optionsBuilderMoviesContext;

		//TODO: still works if private?
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

		//TODO: this must be merged with config
		private static string GetConnectionString()
		{
			var conf = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory + Assembly.GetCallingAssembly().GetName().Name + ".dll");

			var connString = conf.ConnectionStrings.ConnectionStrings["SqlServerConnectionString"].ConnectionString;
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