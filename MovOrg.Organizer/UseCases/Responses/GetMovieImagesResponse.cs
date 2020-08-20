using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetMovieImagesResponse : ResponseBase
	{
		public IEnumerable<MovieImageDto> MovieImages { get; }

		public GetMovieImagesResponse(IEnumerable<MovieImageDto> movieImages) : base()
		{
			MovieImages = movieImages;
		}

		public GetMovieImagesResponse(string error) : base(error)
		{
		}
	}
}