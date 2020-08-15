using Microsoft.VisualStudio.TestTools.UnitTesting;

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