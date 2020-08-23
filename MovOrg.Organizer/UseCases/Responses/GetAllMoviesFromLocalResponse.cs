using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetAllMoviesFromLocalResponse : ResponseBase
	{
		public IEnumerable<MovieListItemDto> Movies { get; set; }

		public GetAllMoviesFromLocalResponse()
		{
		}

		public GetAllMoviesFromLocalResponse(IEnumerable<MovieListItemDto> movies) : base(null)
		{
			Movies = movies;
		}

		public GetAllMoviesFromLocalResponse(string error) : base(error)
		{
		}
	}
}