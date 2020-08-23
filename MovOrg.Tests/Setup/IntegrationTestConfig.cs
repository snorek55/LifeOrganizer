using Common.Setup;

using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace MovOrg.Tests.Setup
{
	public class IntegrationTestConfig : IConfig
	{
		private Configuration configuration;

		public IntegrationTestConfig()
		{
			configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().GetName().Name + ".dll");
		}

		public Task AddSearchedTitleAsync(string suggestedTitle)
		{
			throw new System.NotImplementedException();
		}

		public string GetConnectionString(bool isMigrations)
		{
			return @"Data Source=(LocalDB)\MSSQLLocalDB;Database=LifeOrganizerDBTest;";
		}

		public string GetIMDbApiKey()
		{
			return configuration.AppSettings.Settings["IMDbApiKey"].Value;
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