using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.Unit;
using Tests.Unit.Infrastructure;

namespace Tests.Integration
{
	[TestClass]
	public class SqlServerMoviesTests : LocalRepoMoviesTests
	{
		public SqlServerMoviesTests() : base(IntegrationConfig)
		{
		}
	}
}