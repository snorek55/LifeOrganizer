using Common.UseCases;

using MovOrg.Organizers.Domain;

using System.Collections.Generic;

namespace MovOrg.Organizers.UseCases.Responses
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