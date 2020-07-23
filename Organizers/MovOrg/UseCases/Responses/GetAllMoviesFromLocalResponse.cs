using Domain;

using Organizers.Common.UseCases;

using System.Collections.Generic;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class GetAllMoviesFromLocalResponse : ResponseBase
	{
		public IEnumerable<Movie> Movies { get; internal set; }

		public GetAllMoviesFromLocalResponse(IEnumerable<Movie> movies) : base(null)
		{
			Movies = movies;
		}

		public GetAllMoviesFromLocalResponse(string error) : base(error)
		{
		}
	}
}