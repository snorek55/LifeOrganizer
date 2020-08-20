using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface ILocalMoviesRepository : IReadOnlyMoviesRepository
	{
		Task<bool> AreDetailsAvailableFor(string id);

		Task<IEnumerable<Movie>> GetAllMovies();

		Task UpdateSuggestedTitleMovies(IEnumerable<Movie> movies);

		Task UpdateMovieDetails(UpdateMovieDetailsDto movie);

		Task UpdateTopMovies(IEnumerable<Movie> topApiMovies);

		Task<IEnumerable<RatingSource>> GetRatingSources();

		void MarkMovieAsFavorite(string id, bool isFavorite);

		void MarkMovieAsMustWatch(string id, bool isMustWatch);

		void MarkMovieAsWatched(string id, bool isWatched);

		IEnumerable<MovieImageDto> GetMovieImagesById(string id);
	}
}