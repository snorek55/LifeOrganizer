using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.DbAccess
{
	public interface IMovieDetailsDbAccess
	{
		Task<MovieWithDetailsDto> GetMovieDetails(string id);

		Task AddMovieDetails(UpdateMovieDetailsDto movieDetails);

		Task<bool> AreDetailsAvailableFor(string movieId);

		Task<IEnumerable<MovieImageDto>> GetMovieImages(string movieId);

		void MarkMovieAsFavorite(string id, bool isFavorite);

		void MarkMovieAsMustWatch(string id, bool isMustWatch);

		void MarkMovieAsWatched(string id, bool isWatched);

		IEnumerable<MovieRatingSourceDto> GetRatingSourceUrls(string id);
		void UpdateSourcesWebPages(string id, IEnumerable<MovieRatingSourceDto> sourcesWebPages);
	}
}