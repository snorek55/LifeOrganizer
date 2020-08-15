using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Tests.Unit.Infrastructure;

namespace MovOrg.Tests.Integration
{
	[TestClass]
	public class SqlServerMoviesTests : LocalRepoMoviesTests
	{
		public SqlServerMoviesTests() : base(IntegrationConfig)
		{
		}
	}
}