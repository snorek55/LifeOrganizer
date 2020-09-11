using Common;
using Common.Setup;
using Common.UseCases;

using IMDbApiLib;
using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Repositories;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.APIs
{
	public class MovieDetailsApiAccess : IMovieDetailsApiAccess
	{
		private readonly ApiLib apiLib;
		private IAutoMapper mapper;
		private ILocalMoviesRepository localMoviesRepository;

		public MovieDetailsApiAccess(IAutoMapper mapper, IConfig config, ILocalMoviesRepository localMoviesRepository)
		{
			CultureInfo.CurrentCulture = new CultureInfo("en-US");
			this.mapper = mapper;
			this.localMoviesRepository = localMoviesRepository;
			apiLib = new ApiLib(config.GetIMDbApiKey());
		}

		public async Task<UpdateMovieDetailsDto> GetMovieDetails(string id)
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
	}
}