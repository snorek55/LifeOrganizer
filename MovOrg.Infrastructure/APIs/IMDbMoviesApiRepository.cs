using Common;
using Common.Setup;
using Common.UseCases;

using IMDbApiLib;
using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.APIs
{
	public class IMDbMoviesApiRepository : IApiMoviesRepository
	{
		private readonly ILocalMoviesRepository localMoviesRepository;

		private readonly ApiLib apiLib;
		private IAutoMapper mapper;

		public IMDbMoviesApiRepository(IAutoMapper mapper, ILocalMoviesRepository localMoviesRepository, IConfig config)
		{
			CultureInfo.CurrentCulture = new CultureInfo("en-US");
			this.mapper = mapper;
			apiLib = new ApiLib(config.GetIMDbApiKey());
			this.localMoviesRepository = localMoviesRepository;
		}

		public async Task<IEnumerable<MovieListItemDto>> GetMoviesFromSuggestedTitle(string suggestedTitle)
		{
			Ensure.IsNotNull(suggestedTitle);
			var data = await apiLib.SearchMovieAsync(suggestedTitle);
			ThrowIfError(data.ErrorMessage);
			return mapper.Map<IEnumerable<MovieListItemDto>>(data.Results);
		}

		public async Task<UpdateMovieDetailsDto> GetAllMovieDetailsById(string id)
		{
			Ensure.IsNotNull(id);
			var data = await apiLib.TitleAsync(id, Language.en, true, true, true, true, true, true, true);
			ThrowIfError(data.ErrorMessage);
			var movie = mapper.Map<Movie>(data);
			//TODO: updateratings must be as dto
			await UpdateRatings(data.Ratings, movie);

			var dto = mapper.Map<UpdateMovieDetailsDto>(movie);

			return dto;
		}

		public async Task<IEnumerable<MovieListItemDto>> GetTopMovies()
		{
			var data = await apiLib.Top250MoviesAsync();
			ThrowIfError(data.ErrorMessage);
			var movies = mapper.Map<IEnumerable<MovieListItemDto>>(data.Items);
			return movies;
		}

		public async Task<Person> GetPersonDetails(string id)
		{
			Ensure.IsNotNull(id);
			var data = await apiLib.NameAsync(id);
			ThrowIfError(data.ErrorMessage);
			throw new NotImplementedException();
		}

		private async Task UpdateRatings(RatingData data, Movie movie)
		{
			var ratings = new List<Rating>();
			var sources = await localMoviesRepository.GetRatingSources();
			var i = 1;
			foreach (var source in sources)
			{
				var property = data.GetType().GetProperties().ToList().Single(x => x.Name.Equals(source.Name, StringComparison.OrdinalIgnoreCase));
				var score = (string)property.GetValue(data);
				float? floatScore = null;

				if (!string.IsNullOrEmpty(score))
					floatScore = float.Parse(score);

				var rating = new Rating
				{
					Source = source,
					SourceId = source.Id,
					Movie = movie,
					MovieId = movie.Id,
					Id = i.ToString(),
					Score = floatScore
				};

				ratings.Add(rating);
				i++;
			}

			movie.Ratings = ratings;
		}

		private static void ThrowIfError(string errorMessage)
		{
			if (!string.IsNullOrEmpty(errorMessage)) throw new RepositoryException(errorMessage);
		}

		public async Task<IEnumerable<MovieRatingSourceDto>> GetRatingSourcesUrls(string id)
		{
			//TODO: make automatic with reflection or smh
			var externalSiteData = await apiLib.ExternalSitesAsync(id);
			var relevantRatingSites = new List<MovieRatingSourceDto>
			{
				new MovieRatingSourceDto
				{
					SourceUrl = externalSiteData.RottenTomatoes.Url,
					SourceName = "RottenTomatoes"
				},
				new MovieRatingSourceDto
				{
					SourceUrl = externalSiteData.Metacritic.Url,
					SourceName = "Metacritic"
				},
				new MovieRatingSourceDto
				{
					SourceUrl = externalSiteData.TV_com.Url,
					SourceName = "TV_com"
				},
				new MovieRatingSourceDto
				{
					SourceUrl = externalSiteData.IMDb.Url,
					SourceName = "IMDb"
				}
			};

			return relevantRatingSites;
		}
	}
}