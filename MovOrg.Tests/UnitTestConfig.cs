using Common.Setup;

namespace Tests.Common
{
	public class UnitTestConfig : Config
	{
		public override string GetConnectionString()
		{
			//SQLite in memory db
			return "DataSource =:memory:";
		}

		public override string GetIMDbApiKey()
		{
			throw new System.NotImplementedException();
		}
	}
}