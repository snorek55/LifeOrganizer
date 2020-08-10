using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Infrastructure
{
	[TestClass]
	public class InMemoryRepoMoviesTests : LocalRepoMoviesTests
	{
		public InMemoryRepoMoviesTests() : base(UnitTestConfig)
		{
		}
	}
}