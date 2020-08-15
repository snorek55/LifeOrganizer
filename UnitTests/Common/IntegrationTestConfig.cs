using Common.Setup;

using System.Configuration;

namespace Tests.Common
{
	public class IntegrationTestConfig : Config
	{
		public IntegrationTestConfig()
		{
			Configuration = ConfigurationManager.OpenExeConfiguration(@"Main.GUI.dll");
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