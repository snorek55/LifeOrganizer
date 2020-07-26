using Organizers.Common.Config;

using System.Threading.Tasks;

namespace UnitTests
{
	public class TestConfig : IConfig
	{
		public string GetConnectionString()
		{
			return "Filename=Test.db";
		}

		public string GetIMDbApiKey()
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> WasAlreadySearched(string term)
		{
			throw new System.NotImplementedException();
		}
	}
}