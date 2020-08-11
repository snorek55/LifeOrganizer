using System.Threading.Tasks;

namespace Common.Config
{
	public interface IConfig
	{
		Task<bool> WasAlreadySearched(string term);

		string GetConnectionString();

		string GetIMDbApiKey();

		Task AddSearchedTitleAsync(string suggestedTitle);

		string GetRatingSourceLogoUrl(string ratingSourceName);
	}
}