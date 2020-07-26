﻿using Organizers.Common.UseCases;
using Organizers.MovOrg.Domain;

using System;
using System.Collections.Generic;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class GetSuggestedTitleMoviesResponse : ResponseBase
	{
		public IEnumerable<Movie> Movies { get; private set; }

		public bool AlreadySearched { get; set; }

		public GetSuggestedTitleMoviesResponse()
		{
		}

		public GetSuggestedTitleMoviesResponse(IEnumerable<Movie> movies) : base()
		{
			Movies = movies ?? throw new ArgumentNullException(nameof(movies));
		}

		public GetSuggestedTitleMoviesResponse(string error) : base(error)
		{
		}
	}
}