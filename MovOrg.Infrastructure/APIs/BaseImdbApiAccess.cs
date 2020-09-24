using Common.Setup;
using Common.UseCases;

using IMDbApiLib;

using System.Globalization;

namespace MovOrg.Infrastructure.APIs
{
	public abstract class BaseImdbApiAccess
	{
		protected readonly ApiLib apiLib;
		protected readonly IAutoMapper mapper;

		protected BaseImdbApiAccess(IAutoMapper mapper, IConfig config)
		{
			CultureInfo.CurrentCulture = new CultureInfo("en-US");
			this.mapper = mapper;
			apiLib = new ApiLib(config.GetIMDbApiKey());
		}

		protected static void ThrowIfError(string errorMessage)
		{
			if (!string.IsNullOrEmpty(errorMessage)) throw new RepositoryException(errorMessage);
		}
	}
}