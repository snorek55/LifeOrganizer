using EntryPoint;

namespace Tests.Common
{
	public class IntegrationTestConfig : Config
	{
		public override string GetConnectionString()
		{
			return @"Data Source=(LocalDB)\MSSQLLocalDB;Database=LifeOrganizerDBTest;";
		}

		public override string GetIMDbApiKey()
		{
			throw new System.NotImplementedException();
		}
	}
}