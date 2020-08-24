using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

using System;
using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetSuggestedTitleMoviesResponse : ResponseBase
	{
		public IEnumerable<MovieListItemDto> Movies { get; set; }

		public bool AlreadySearched { get; set; }

		public GetSuggestedTitleMoviesResponse()
		{
		}

		public GetSuggestedTitleMoviesResponse(IEnumerable<MovieListItemDto> movies) : base()
		{
			Movies = movies ?? throw new ArgumentNullException(nameof(movies));
		}

		public GetSuggestedTitleMoviesResponse(string error) : base(error)
		{
		}
	}
}