using Common.UseCases;

using Organizers.MovOrg.Domain;

using System.Collections.Generic;

namespace Organizers.MovOrg.UseCases.Responses
{
	public class GetAllMoviesFromLocalResponse : ResponseBase
	{
		public IEnumerable<Movie> Movies { get; set; }

		public GetAllMoviesFromLocalResponse()
		{
		}

		public GetAllMoviesFromLocalResponse(IEnumerable<Movie> movies) : base(null)
		{
			Movies = movies;
		}

		public GetAllMoviesFromLocalResponse(string error) : base(error)
		{
		}
	}
}