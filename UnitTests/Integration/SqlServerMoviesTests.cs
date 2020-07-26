using EntryPoint;

using Infrastructure.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration
{
	[TestClass]
	public class SqlServerMoviesTests : LocalRepoMoviesTests
	{
		public SqlServerMoviesTests() : base(
		new DbContextOptionsBuilder<MoviesContext>()
					.UseSqlServer(new Config().GetConnectionString())
					.Options)
		{
		}
	}
}