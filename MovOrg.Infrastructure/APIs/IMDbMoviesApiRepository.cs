﻿using Common;
using Common.Setup;
using Common.UseCases;

using IMDbApiLib;
using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;
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

		public async Task<IEnumerable<Movie>> GetMoviesFromSuggestedTitle(string suggestedTitle)
		{
			Ensure.IsNotNull(suggestedTitle);
			var data = await apiLib.SearchMovieAsync(suggestedTitle);
			ThrowIfError(data.ErrorMessage);
			return mapper.Map<IEnumerable<Movie>>(data.Results);
		}

		public async Task<Movie> GetMovieDetailsById(string id)
		{
			Ensure.IsNotNull(id);
			var data = await apiLib.TitleAsync(id, Language.en, true, true, true, true, true, true, true);
			ThrowIfError(data.ErrorMessage);
			var movie = mapper.Map<Movie>(data);

			int i = 0;
			foreach (var image in data.Images.Items)
			{
				movie.Images.Add(new MovieImageData
				{
					Id = (i++).ToString(),
					MovieId = data.Images.IMDbId,
					Movie = movie,
					Image = image.Image,
					Title = image.Title
				});
			}

			foreach (var actor in movie.ActorList)
			{
				if (data.StarList.Any(x => x.Id == actor.PersonId))
					actor.IsStar = true;
			}

			await UpdateRatings(data.Ratings, movie);
			return movie;
		}

		public async Task<IEnumerable<Movie>> GetTopMovies()
		{
			var data = await apiLib.Top250MoviesAsync();
			ThrowIfError(data.ErrorMessage);
			var movies = mapper.Map<IEnumerable<Movie>>(data.Items);
			foreach (var movie in movies)
			{
				movie.LastUpdatedTop250 = DateTime.Now;
			}

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
	}
}