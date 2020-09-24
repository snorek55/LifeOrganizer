using Common;
using Common.Setup;

using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DbAccess;
using MovOrg.Organizer.UseCases.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovOrg.Infrastructure.APIs
{
	public class ImdbMovieDetailsApiAccess : BaseImdbApiAccess, IMovieDetailsApiAccess
	{
		private IMoviesListsDbAccess listsDbAccess;

		public ImdbMovieDetailsApiAccess(IAutoMapper mapper, IConfig config, IMoviesListsDbAccess listsDbAccess) : base(mapper, config)
		{
			this.listsDbAccess = listsDbAccess;
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
			var sources = await listsDbAccess.GetRatingSources();
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
	}
}