using System.Threading.Tasks;

namespace Organizers.Common.Config
{
	public interface IConfig
	{
		Task<bool> WasAlreadySearched(string term);

		string GetConnectionString();

		string GetIMDbApiKey();
	}
}