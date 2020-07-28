using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit
{
	[TestClass]
	public class InMemoryRepoMoviesTests : LocalRepoMoviesTests
	{
		public InMemoryRepoMoviesTests() : base(UnitTestConfig)
		{
		}
	}
}