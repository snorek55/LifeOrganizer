using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetMoviesFromLocalResponse : ResponseBase
	{
		public IEnumerable<MovieListItemDto> Movies { get; set; }

		public GetMoviesFromLocalResponse()
		{
		}

		public GetMoviesFromLocalResponse(IEnumerable<MovieListItemDto> movies) : base(null)
		{
			Movies = movies;
		}

		public GetMoviesFromLocalResponse(string error) : base(error)
		{
		}
	}
}