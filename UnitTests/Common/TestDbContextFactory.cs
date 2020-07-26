using EntityFramework.DbContextScope.Interfaces;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;

using Organizers.Common.Config;

using System;

namespace Tests.Common
{
	public class TestDbContextFactory : IDbContextFactory
	{
		private IConfig config;

		public TestDbContextFactory(IConfig config)
		{
			this.config = config;
		}

		public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
		{
			var testOptions = new DbContextOptionsBuilder<MoviesContext>()
					.UseSqlite(config.GetConnectionString())
					.Options;
			if (typeof(TDbContext).Equals(typeof(MoviesContext)))
				return (TDbContext)Activator.CreateInstance(typeof(MoviesContext), testOptions);

			return Activator.CreateInstance<TDbContext>();
		}
	}
}