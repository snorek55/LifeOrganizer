using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Repositories
{
	public interface ILocalMoviesRepository : IReadOnlyMoviesRepository
	{
		Task<bool> AreDetailsAvailableFor(string id);

		Task<IEnumerable<MovieListItemDto>> GetAllMovies();

		Task UpdateSuggestedTitleMovies(IEnumerable<MovieListItemDto> movies);

		Task UpdateMovieDetails(UpdateMovieDetailsDto movie);

		Task UpdateTopMovies(IEnumerable<MovieListItemDto> topApiMovies);

		Task<IEnumerable<RatingSource>> GetRatingSources();

		void MarkMovieAsFavorite(string id, bool isFavorite);

		void MarkMovieAsMustWatch(string id, bool isMustWatch);

		void MarkMovieAsWatched(string id, bool isWatched);

		IEnumerable<MovieImageDto> GetMovieImagesById(string id);

		IEnumerable<MovieRatingSourceDto> GetRatingSourceUrls(string id);

		void UpdateSourcesWebPages(string id, IEnumerable<MovieRatingSourceDto> sourcesWebPages);
	}
}