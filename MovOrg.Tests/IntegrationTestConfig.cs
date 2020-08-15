using Common.Setup;

using System.Configuration;
using System.Reflection;

namespace Tests.Common
{
	public class IntegrationTestConfig : Config
	{
		public IntegrationTestConfig()
		{
			Configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().GetName().Name + ".dll");
		}

		public override string GetConnectionString()
		{
			return @"Data Source=(LocalDB)\MSSQLLocalDB;Database=LifeOrganizerDBTest;";
		}

		public override string GetIMDbApiKey()
		{
			return Configuration.AppSettings.Settings["IMDbApiKey"].Value;
		}
	}
}