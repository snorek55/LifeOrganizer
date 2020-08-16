using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovOrg.Tests.Base;

namespace MovOrg.Tests.Unit.Infrastructure
{
	[TestClass]
	public class InMemoryRepoMoviesTests : LocalRepoMoviesTests
	{
		public InMemoryRepoMoviesTests() : base(UnitTestConfig)
		{
		}
	}
}