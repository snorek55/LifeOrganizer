using Common;
using Common.Setup;
using Common.UseCases;

using IMDbApiLib;

using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.APIs
{
	public class MoviesListsApiAccess : IMoviesListsApiAccess
	{
		private readonly ApiLib apiLib;
		private IAutoMapper mapper;
		private ILocalMoviesRepository localMoviesRepository;

		public MoviesListsApiAccess(IAutoMapper mapper, IConfig config, ILocalMoviesRepository localMoviesRepository)
		{
			CultureInfo.CurrentCulture = new CultureInfo("en-US");
			this.mapper = mapper;
			this.localMoviesRepository = localMoviesRepository;
			apiLib = new ApiLib(config.GetIMDbApiKey());
		}

		//TODO: must change throw if error or smh
		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle)
		{
			Ensure.IsNotNull(suggestedTitle);
			var data = await apiLib.SearchMovieAsync(suggestedTitle);
			ThrowIfError(data.ErrorMessage);
			return mapper.Map<IEnumerable<MovieListItemDto>>(data.Results);
		}

		private static void ThrowIfError(string errorMessage)
		{
			if (!string.IsNullOrEmpty(errorMessage)) throw new RepositoryException(errorMessage);
		}
	}
}