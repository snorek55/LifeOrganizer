using Common.Setup;

using System.Threading.Tasks;

namespace MovOrg.Tests.Setup
{
	public class UnitTestConfig : IConfig
	{
		public Task AddSearchedTitleAsync(string suggestedTitle)
		{
			throw new System.NotImplementedException();
		}

		public string GetConnectionString(bool isMigrations)
		{
			//SQLite in memory db
			return "DataSource =:memory:";
		}

		public string GetIMDbApiKey()
		{
			throw new System.NotImplementedException();
		}

		public string GetRatingSourceLogoUrl(string ratingSourceName)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> WasAlreadySearched(string term)
		{
			throw new System.NotImplementedException();
		}
	}
}