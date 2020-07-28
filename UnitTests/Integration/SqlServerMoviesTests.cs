using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.Unit;

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