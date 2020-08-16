using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Infrastructure.EFCore;
using MovOrg.Tests.Unit.Infrastructure;

using System.Diagnostics;

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